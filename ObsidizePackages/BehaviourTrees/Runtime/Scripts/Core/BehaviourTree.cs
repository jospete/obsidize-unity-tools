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
            var node = CreateInstance(type) as Node;
            return NormalizeAndAdd(node);
        }

        public Node CreateNodeWithRootCheck(System.Type type)
		{
            return type == typeof(RootNode) ? CreateRootNode() : CreateNode(type);
        }

        public Node AddExistingNodeWithRootCheck(Node node)
        {

            if (node is not RootNode)
			{
                NormalizeAndAddDistinct(node);
            }

            return node;
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

        private bool NormalizeAndAddDistinct(Node node)
		{

            if (Children.Contains(node))
			{
                return false;
			}

            NormalizeAndAdd(node);
            return true;
		}

        private Node NormalizeAndAdd(Node node)
        {

            node.name = node.GetType().Name;
            node.Guid = System.Guid.NewGuid().ToString();

            // if we're adding a new node,
            // it should not come with any implicit connections
            node.DetachAllChildren();
            Children.Add(node);

            return node;
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
