using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Obsidize.BehaviourTrees.Editor
{
    public static class BehaviourTreeEditorUtility
    {

        private static BehaviourTreeEditorSettings _settings;

        public static BehaviourTreeEditorSettings Settings
		{
            get => _settings;
            set => _settings = value;
        }

        public static readonly NodeState[] ALL_NODE_STATES = Enum.GetValues(typeof(NodeState)).Cast<NodeState>().ToArray();
        public static string GetGraphViewClassName(this NodeState state) => $"bt-node-state-{state.ToString().ToLower()}";

        public static T CreateNodeAsset<T>(this BehaviourTree tree) where T : Node
		{
            return CreateNodeAsset(tree, typeof(T)) as T;
		}

        public static void CreateRootNodeAssetIfNeeded(this BehaviourTree tree)
		{
            if (tree != null && tree.Root == null)
			{
                CreateNodeAsset(tree, typeof(RootNode));
            }
		}

        public static Node CreateNodeAsset(this BehaviourTree tree, System.Type type)
		{

            var undoLabel = "Behaviour Tree (Create Node)";
            Undo.RecordObject(tree, undoLabel);

            var node = tree.CreateNodeWithRootCheck(type);
            Undo.RegisterCreatedObjectUndo(node, undoLabel);

            if (!Application.isPlaying)
			{
                AssetDatabase.AddObjectToAsset(node, tree);
            }

            return node;
		}

        public static void DeleteNodeAsset(this BehaviourTree tree, Node node)
		{

            var undoLabel = "Behaviour Tree (Delete Node)";
            Undo.RecordObject(tree, undoLabel);

            tree.DeleteNode(node);

            Undo.DestroyObjectImmediate(node);

            if (!Application.isPlaying)
			{
                AssetDatabase.SaveAssets();
            }
		}

        public static void LoadPrimarySettingsAsset()
		{
            Settings = FindAssetsByType<BehaviourTreeEditorSettings>().FirstOrDefault();
		}

        public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {

            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }

            return assets;
        }
    }
}
