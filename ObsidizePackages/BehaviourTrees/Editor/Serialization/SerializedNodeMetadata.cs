using System;
using UnityEditor;
using UnityEngine;

namespace Obsidize.BehaviourTrees.Editor
{
    [Serializable]
    public struct SerializedNodeMetadata
    {

        public string assemblyQualifiedName;
        public string serializedNodeData;

        public Type NodeType => Type.GetType(assemblyQualifiedName);

        public SerializedNodeMetadata(string assemblyQualifiedName, string serializedNodeData)
        {
            this.assemblyQualifiedName = assemblyQualifiedName;
            this.serializedNodeData = serializedNodeData;
        }

        public string ToJsonString()
        {
            return EditorJsonUtility.ToJson(this);
        }

        public Node ToNode()
        {
            var result = ScriptableObject.CreateInstance(NodeType) as Node;
            EditorJsonUtility.FromJsonOverwrite(serializedNodeData, result);
            return result;
        }

        public static SerializedNodeMetadata FromJson(string json)
        {
            var metadata = new SerializedNodeMetadata();
            EditorJsonUtility.FromJsonOverwrite(json, metadata);
            return metadata;
        }

        public static SerializedNodeMetadata FromNode<T>(T node) where T : Node
        {
            return new SerializedNodeMetadata(
                node.GetType().AssemblyQualifiedName,
                EditorJsonUtility.ToJson(node)
            );
        }
    }
}
