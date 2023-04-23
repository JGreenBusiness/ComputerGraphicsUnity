using UnityEngine;

namespace UnityTemplateProjects.GUI
{
	public class ActionListUI : MonoBehaviour
	{
		public ActionList actionList;
		public ActionUI prefab;
		// Start is called before the first frame update
		private void Start()
		{
			foreach (Action _action in actionList.actions)
			{
				// make this a child of ours on creation. 
				// Don't worry about specifying a position as the LayoutGroup handles that
				ActionUI ui = Instantiate(prefab, transform);
				ui.SetAction(_action);
			}
		}

	}
}