using UnityEngine.UIElements;

namespace Obsidize.BehaviourTrees.Editor
{
    public class InspectorView : VisualElement
    {

        public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { }

		private UnityEditor.Editor editor;

        public InspectorView()
		{
		}

		private void OnDrawContainerEditor()
		{

			if (editor != null && editor.target != null)
			{
				editor.OnInspectorGUI();
			}
		}

		internal void SetSelectedNode(BehaviourTreeNodeView nodeView)
		{

			Clear();
			UnityEngine.Object.DestroyImmediate(editor);

			editor = UnityEditor.Editor.CreateEditor(nodeView.Target);
			var container = new IMGUIContainer(OnDrawContainerEditor);

			Add(container);
		}
	}
}
