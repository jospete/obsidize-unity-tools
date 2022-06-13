using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Obsidize.BehaviourTrees.Editor
{

    [CreateAssetMenu(menuName = "Behaviour Trees/Editor Settings")]
    public class BehaviourTreeEditorSettings : ScriptableObject
    {

        [SerializeField]
        private VisualTreeAsset _rootVisualTree;

        [SerializeField]
        private StyleSheet _rootStyleSheet;

        [SerializeField]
        private VisualTreeAsset _nodeViewVisualTree;

        [SerializeField]
        private StyleSheet _nodeViewStyleSheet;

        public VisualTreeAsset RootVisualTree => _rootVisualTree;
        public StyleSheet RootStyleSheet => _rootStyleSheet;
        public VisualTreeAsset NodeViewVisualTree => _nodeViewVisualTree;
        public StyleSheet NodeViewStyleSheet => _nodeViewStyleSheet;
        public string NodeViewUxmlPath => AssetDatabase.GetAssetPath(NodeViewVisualTree);
    }
}

