using System.Collections.Generic;
using UnityEngine;

namespace Obsidize.BehaviourTrees.Editor
{
    public static class NodeSerializationUtility
    {

        public static string ToMetaJsonString(this Node node)
        {
            return SerializedNodeMetadata.FromNode(node).ToJsonString();
        }

        public static string ToSerializedNodeList(this IEnumerable<Node> nodes)
        {
            var collection = SerializedNodeMetadataCollection.From(nodes);
            return JsonUtility.ToJson(collection);
        }

        public static IEnumerable<Node> DeserializeAllNodes(string metadataCollection)
        {
            var collection = JsonUtility.FromJson<SerializedNodeMetadataCollection>(metadataCollection);
            return collection.ParseNodes();
        }
    }
}
