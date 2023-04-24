using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(PlayerInput)),RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    // Set on awake
    private Animator _animator;
    private PlayerInput _playerInput;
    private CharacterController _characterController;
    private Camera _camera;
    
    // Set in inspector
    [SerializeField] private CinemachineVirtualCamera virtualCameraCameraThatOverides;
    [SerializeField] private LayerMask aimingLayerMask = new LayerMask();
    [SerializeField] private bool idCursorLocked = true;
    [SerializeField] private float smoothTime = 1;
    [SerializeField] private float rotationSpeed = 10;

    // This will contain basic controls based pm pir key inputs
    private Vector2 _direction;
    private bool _isMoving;
    private bool _isSprinting;
    private bool _isAiming ;
    

    // Private fields
    private float _speed;
    private Vector2 _playerVelocity;
    private Vector2 _smoothedPlayerVelocity;
    private Vector3 _aimTarget;

    // Public fields
    public float movementSpeed = 1f;
    public float sprintSpeed = 3f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _characterController = GetComponent<CharacterController>();
        _camera = Camera.main;
    }

    private void Start()
    {
        SetCursorLockState(true);
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        _direction = value.ReadValue<Vector2>();
        Debug.Log("Move: " + _direction);
        

        if (_direction != Vector2.zero)
        {
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
            
        }
    }

    public void OnSprint(InputAction.CallbackContext value)
    {
        // This way true is called twice
        //_isSprinting = value.ReadValueAsButton();
        
        if (value.started)
        {
            _isSprinting = true;
            Debug.Log("Sprinting: " + _isSprinting);
        }
        else if (value.canceled)
        {
            _isSprinting = false;
            Debug.Log("Sprinting: " + _isSprinting);
        }

    }
    
    public void OnAim(InputAction.CallbackContext value)
    {
        _isAiming = value.ReadValueAsButton();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _speed = _isSprinting ? sprintSpeed :
                    !_isMoving ? 0 : movementSpeed;
        

        if (!_isAiming)
        {
            CasualMovement(Time.fixedDeltaTime);
            SetAnimationActiveLayer(_animator,1,0,Time.fixedDeltaTime,10);
            SetAnimationActiveLayer(_animator,2,0,Time.fixedDeltaTime,10);
            virtualCameraCameraThatOverides.Priority = 9;
        }
        else
        {
            virtualCameraCameraThatOverides.Priority = 11;
            SetAnimationActiveLayer(_animator,1,1,Time.fixedDeltaTime,10);
            SetAnimationActiveLayer(_animator,2,1,Time.fixedDeltaTime,10);
        }

        _playerVelocity = Vector2.SmoothDamp(_playerVelocity,_direction,ref _smoothedPlayerVelocity,smoothTime);
        
        Vector3 move = new Vector3(_playerVelocity.x, 0, _playerVelocity.y);
        
        move = move.x * _camera.transform.right.normalized + move.z * _camera.transform.forward.normalized;
        move.y = 0;
        
        _characterController.Move(move.normalized * (Time.fixedDeltaTime * _speed));

        Vector2 myScreenCentre = new Vector2(Screen.width*.5f, Screen.height*.5f);
        Ray ray = _camera.ScreenPointToRay(myScreenCentre);
        if (Physics.Raycast(ray,out RaycastHit raycastHit,200,aimingLayerMask))
        {
            _aimTarget = raycastHit.point;
        }
        else
        {
            _aimTarget = ray.GetPoint(200);
        }

        _aimTarget.y = transform.position.y;
        Vector3 aimDirection = (_aimTarget - transform.position).normalized;
        if (move != Vector3.zero && !_isAiming)
        {
            transform.forward = move;
            _playerVelocity.y = 0;
            _playerVelocity.x = 0;
        }
        else if (_isAiming)
        {
            transform.forward = Vector3.Lerp(transform.forward,aimDirection,Time.fixedDeltaTime*20);
        }

        //Quaternion rotation = Quaternion.Euler(0,_camera.transform.eulerAngles.y,0);
        //transform.rotation = Quaternion.Lerp(transform.rotation,rotation,Time.fixedDeltaTime * rotationSpeed);
    }

    private void LateUpdate()
    {
        _animator.SetFloat("ForwardMotion", _playerVelocity.y);
        _animator.SetFloat("RightMotion", _playerVelocity.x);
        _animator.SetBool("IsMoving", _isMoving);
    }

    public void CasualMovement(float dt)
    {
        if (_isMoving)
        {
            _animator.SetFloat("Speed", _speed,0.1f,dt);
        }
        else
        {
            _animator.SetFloat("Speed",0);
        }
    }

    private void SetAnimationActiveLayer(Animator animator, int layer, int transitionValue, float dt, float rateOfChange)
    {
        animator.SetLayerWeight(layer,Mathf.Lerp(animator.GetLayerWeight(layer), transitionValue, dt * rateOfChange));
    }

    private void SetCursorLockState(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !state;
    }
}
