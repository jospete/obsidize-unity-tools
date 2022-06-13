using UnityEditor;
using UnityEngine;

namespace Obsidize.BehaviourTrees.Editor
{
    [CustomEditor(typeof(Node), true)]
    public class NodeEditor : UnityEditor.Editor
    {

		public override void OnInspectorGUI()
		{

			base.OnInspectorGUI();

			var node = target as Node;

			if (node == null)
			{
				return;
			}

			var children = node.GetChildren();

			if (children == null)
			{
				return;
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Children", EditorStyles.boldLabel);

			if (children.Count <= 0)
			{
				EditorGUILayout.LabelField("None");
				return;
			}

			var previouslyEnabled = GUI.enabled;

			GUI.enabled = false;

			foreach (var child in children)
			{
				EditorGUILayout.ObjectField(child, typeof(Node), false);
			}

			GUI.enabled = previouslyEnabled;
		}
	}
}
