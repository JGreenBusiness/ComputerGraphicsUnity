using UnityEngine;

namespace UnityTemplateProjects.GUI
{
	public class ActionList : MonoBehaviour
	{
		[SerializeField] private Action[] _actions = null;
		// Start is called before the first frame update
		public Action[] actions
		{
			get
			{
				if (_actions == null)
					_actions = GetComponents<Action>();
				return _actions;
			}
		}

	}
}