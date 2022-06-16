using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Obsidize.BehaviourTrees.Editor
{
    [Serializable]
    public class SerializedNodeMetadataCollection
    {
        
        [SerializeField]
        private List<SerializedNodeMetadata> _items = new List<SerializedNodeMetadata>();

        public List<SerializedNodeMetadata> Items => _items;

        public static SerializedNodeMetadataCollection From(IEnumerable<Node> nodes)
		{
            var collection = new SerializedNodeMetadataCollection();
            collection.Items.AddRange(nodes.Select(SerializedNodeMetadata.FromNode));
            return collection;
		}

		public IEnumerable<Node> ParseNodes()
		{
            return Items.Select(metadata => metadata.ToNode());
		}
	}
}
