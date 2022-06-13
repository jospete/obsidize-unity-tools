using UnityEditor;
using UnityEngine;

namespace Obsidize.BehaviourTrees.Editor
{
    [CustomEditor(typeof(BehaviourTree))]
    public class BehaviourTreeAssetEditor : UnityEditor.Editor
    {

		public override void OnInspectorGUI()
		{
			var previouslyEnabled = GUI.enabled;
			GUI.enabled = false;
			base.OnInspectorGUI();
			GUI.enabled = previouslyEnabled;
		}
	}
}
