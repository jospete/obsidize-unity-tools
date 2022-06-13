using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;

namespace Obsidize.BehaviourTrees.Editor
{
    public class BehaviourTreeEditor : EditorWindow
    {

        private delegate bool GraphViewPopulationStrategy(out BehaviourTree tree);

        private BehaviourTreeGraphView graphView;
        private InspectorView inspectorView;
        private BehaviourTree runtimeSelectionPrefab;

        [MenuItem("Behaviour Trees/Editor")]
        public static void OpenWindow()
        {
            BehaviourTreeEditor window = GetWindow<BehaviourTreeEditor>();
            window.titleContent = new GUIContent("Behaviour Tree Editor");
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int _instanceId, int _line)
		{

            if (Selection.activeObject is BehaviourTree)
			{
                OpenWindow();
                return true;
			}

            return false;
		}

        public void CreateGUI()
        {

            BehaviourTreeEditorUtility.LoadPrimarySettingsAsset();

            if (BehaviourTreeEditorUtility.Settings == null)
			{
                Debug.LogWarning("Failed to load Behaviour Tree Editor Settings");
                return;
			}

            var root = rootVisualElement;
            var visualTree = BehaviourTreeEditorUtility.Settings.RootVisualTree;
            var styleSheet = BehaviourTreeEditorUtility.Settings.RootStyleSheet;

            visualTree.CloneTree(root);
            root.styleSheets.Add(styleSheet);

            graphView = root.Q<BehaviourTreeGraphView>();
            inspectorView = root.Q<InspectorView>();
            graphView.OnNodeSelected = OnNodeSelectionChanged;

            OnSelectionChange();
        }

		private void OnEnable()
		{
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

		private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

		private void OnInspectorUpdate()
		{
			if (graphView != null)
			{
                graphView.SyncAllNodeStates();
			}
		}

		private void OnSelectionChange()
		{

            if (graphView != null)
			{
                graphView.ClearNodesAndConnections();
			}

            if (inspectorView != null)
			{
                inspectorView.Clear();
			}

            if (Application.isPlaying)
			{
                TryPopulateGraphView(TryGetSelectedTreeRuntimeInstance);
            }
            else if (runtimeSelectionPrefab != null)
            {
                TryPopulateGraphView(TryGetRuntimeSelectionPrefab);
            }
            else
			{
                TryPopulateGraphView(TryGetSelectedTreeAsset);
            }
		}

        private void OnNodeSelectionChanged(BehaviourTreeNodeView nodeView)
		{

            if (inspectorView != null)
			{
                inspectorView.SetSelectedNode(nodeView);
			}
        }

        private void TryPopulateGraphView(GraphViewPopulationStrategy strategey)
		{
            if (graphView != null && strategey(out var tree))
            {
                graphView.PopulateView(tree);
            }
		}

        private bool TryGetSelectedTreeAsset(out BehaviourTree tree)
		{
            tree = Selection.activeObject as BehaviourTree;
            runtimeSelectionPrefab = null;
            return tree != null && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID());
        }

        private bool TryGetRuntimeSelectionPrefab(out BehaviourTree tree)
        {
            tree = runtimeSelectionPrefab;
            return tree != null;
        }

        private bool TryGetSelectedTreeRuntimeInstance(out BehaviourTree tree)
		{

            tree = null;

            var controller = Selection.activeGameObject != null
                ? Selection.activeGameObject.GetComponent<BehaviourTreeController>()
                : null;

            if (controller != null)
			{
                tree = controller.ActiveTree;
                runtimeSelectionPrefab = controller.TreePrefab;
            }

            return tree != null;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange mode)
        {
            switch (mode)
            {
                case PlayModeStateChange.EnteredEditMode:
                case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;
                default:
                    break;
            }
        }
    }
}