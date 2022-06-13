using System.Collections.Generic;
using UnityEngine;

namespace Obsidize.BehaviourTrees
{

    [CreateAssetMenu(menuName = "Behaviour Trees/Behaviour Tree")]
    public class BehaviourTree : ScriptableObject
    {

        [SerializeField]
        private RootNode _root;

        [SerializeField]
        private List<Node> _children = new List<Node>();

        private NodeState _treeState = NodeState.Running;

        public NodeState State => _treeState;
        public List<Node> Children => _children;
        public RootNode Root => _root;

        public BehaviourTree Clone()
		{
            var result = Instantiate(this);
            result._root = _root.Clone() as RootNode;
            result.SyncNodeListToRoot();
            return result;
		}

        public void DeleteNode(Node node)
        {
            Children.Remove(node);
        }

        public T CreateNode<T>() where T : Node
		{
            return CreateNode(typeof(T)) as T;
		}

        public Node CreateNode(System.Type type)
		{
            var node = CreateNodeInstance(type);
            Children.Add(node);
            return node;
        }

        public Node CreateNodeWithRootCheck(System.Type type)
		{
            return type == typeof(RootNode) ? CreateRootNode() : CreateNode(type);
		}

        public RootNode CreateRootNode()
		{

            if (_root == null)
			{
                _root = FindExistingRootNode();
			}

            if (_root == null)
			{
                _root = CreateNode<RootNode>();
            }

            return _root;
		}

        public NodeState Update()
        {

            var root = Root;

            if (root == null)
            {
                return NodeState.Failure;
            }

            if (root.State == NodeState.Running)
            {
                _treeState = root.Update();
            }

            return _treeState;
        }

        private RootNode FindExistingRootNode()
		{

			foreach (var node in _children)
			{
                if (node is RootNode)
				{
                    return node as RootNode;
				}
			}

            return null;
		}

        private Node CreateNodeInstance(System.Type type)
		{
            var node = CreateInstance(type) as Node;
            node.name = type.Name;
            node.Guid = System.Guid.NewGuid().ToString();
            return node;
        }

        private void SyncNodeListToRoot()
        {
            _children.Clear();
            Traverse(Root, _children.Add);
        }

        private void Traverse(Node node, System.Action<Node> visit)
		{

            if (node == null)
			{
                return;
			}

            visit(node);

            var children = node.GetChildren();

            if (children == null)
			{
                return;
			}

            foreach (var child in children)
			{
                Traverse(child, visit);
			}
		}
    }
}
