using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class GeneratePlane : MonoBehaviour
{
    [SerializeField] private int density = 64;
    [SerializeField] private float spacing = .25f;
    
    
    private MeshFilter filter;
    private new MeshRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        filter = gameObject.GetComponent<MeshFilter>();
        renderer = gameObject.GetComponent<MeshRenderer>();

        filter.mesh = CreateMesh();
        renderer.material = CreateMaterial();
    }

    private Material CreateMaterial()
    {
        Material material = new Material(Shader.Find("Unlit/Texture"));
        
        material.SetTexture("_MainTex",CreateTexture());

        return material;

    }

    private Texture2D CreateTexture()
    {
        Texture2D texture = new(density, density, TextureFormat.RGBA64, false)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };

        for (int x = 0; x < density; x++)
        {
            for (int y = 0; y < density; y++)
            {
                float n = noise.cnoise(new float2(x * spacing, y * spacing));
                
                texture.SetPixel(x,y,Color.Lerp(Color.black,Color.white, 
                    NoiseFunctions.Remap(n,-1f,1f,0f,1f)));
            }
        }
        
        texture.Apply();

        return texture;
    }

    private Mesh CreateMesh()
    {
        Vector3[] verts = new Vector3[density * density];
        Vector2[] uvs = new Vector2[verts.Length];
        Vector3[] normals = new Vector3[verts.Length];
        int[] triangle = new int[(density -1) * (density - 1) * 6];

        int triangleIndex = 0; 
            
        for (int x = 0; x < density; x++)
        {
            for (int y = 0; y < density; y++)
            {
                int index = x * density + y;

                verts[index] = new Vector3
                {
                    x = x *spacing,
                    y = 0,
                    z = y *spacing
                };

                uvs[index] = new Vector2
                {
                    x = x / (float) density,
                    y = y / (float) density
                };

                normals[index] = Vector3.up;

                if (x < density - 1 && y < density - 1)
                {
                    triangle[triangleIndex ++] = index + 0;
                    triangle[triangleIndex ++] = index + density + 1;
                    triangle[triangleIndex ++] = index + density;
                
                    triangle[triangleIndex ++] = index  + 0;
                    triangle[triangleIndex ++] = index  + 1;
                    triangle[triangleIndex ++] = index +  density + 1;
                }

            }

            
            

        }
        
        Mesh mesh = new()
        {
            vertices = verts,
            normals = normals,
            uv =  uvs,
            triangles = triangle
        };
            
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        return mesh;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
