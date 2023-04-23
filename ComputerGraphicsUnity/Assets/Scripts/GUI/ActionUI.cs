using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace UnityTemplateProjects.GUI
{
	public class ActionUI : MonoBehaviour
	{
		public Action action;
		[Header("Child Components")]
		[SerializeField] private Image icon;
		[SerializeField] private TextMeshProUGUI nameTag;
		[SerializeField] private TextMeshProUGUI descriptionTag;

		private void Start()
		{
			SetAction(action);
		}

		public void SetAction(Action _action)
		{
			if(_action != null)
			{
				if(nameTag)
				{
					nameTag.text = _action.actionName;
				}

				if(descriptionTag)
				{
					descriptionTag.text = _action.description;
				}
				
				if (icon)
				{
					icon.sprite = _action.icon;
					icon.color = _action.color;
				}
			}
		}
	}
	
	
}