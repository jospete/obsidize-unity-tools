using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Obsidize.BehaviourTrees
{

	public abstract class CompositeNode<T> : CompositeNode, INode<T> where T : Component
	{
		public T ControllerState { get; private set; }

		public override void OnTreeAwake(BehaviourTreeController tree)
		{
			base.OnTreeAwake(tree);
			ControllerState = tree.GetComponent<T>();
		}
	}

	public abstract class CompositeNode : Node
    {
        
        [SerializeField]
		[HideInInspector]
		private List<Node> _children = new List<Node>();

        public List<Node> Children => _children;

		private int _currentIndex;

		protected abstract NodeState TerminalChildState { get; }
		protected abstract NodeState AllChildrenProcessedState { get; }

		public override string PrimaryUssClass => "bt-composite";
		public sealed override NodeChildCapacity ChildCapacity => NodeChildCapacity.Multi;

		protected override void OnStart()
		{
			_currentIndex = 0;
		}

		public override bool DetachChild(Node node)
		{
			return Children.Remove(node);
		}

		public override List<Node> GetChildren()
		{
			return Children;
		}

		protected virtual void OnValidate()
		{
			_children.RemoveAll(x => x == null);
		}

		public void SortChildrenByGraphPositionHorizontal()
		{
			_children.Sort(CompareGraphPositionsHorizontal);
		}

		private int CompareGraphPositionsHorizontal(Node left, Node right)
		{
			return left.GraphPosition.x < right.GraphPosition.x ? -1 : 1;
		}

		public override Node Clone()
		{
			var result = Instantiate(this);

			result._children = _children
				.Where(x => x != null)
				.Select(x => x.Clone())
				.ToList();

			return result;
		}

		public override bool AttachChild(Node child)
		{

			var canAdd = !Children.Contains(child);

			if (canAdd)
			{
				Children.Add(child);
			}

			return canAdd;
		}

		protected override NodeState OnUpdate()
		{

			if (!Controller.ActiveTree.Root.PersistCompositeState)
			{
				return EvaluateAllChildren();
			}
			
			var isExitState = EvaluateChildAt(_currentIndex, out var updatedState);
			
			if (!isExitState)
			{
				_currentIndex++;
			}

			return updatedState;
		}

		protected NodeState EvaluateAllChildren()
		{

			if (Children.Count > 0)
			{
				for (int i = 0; i < Children.Count; i++)
				{
					if (EvaluateChildAt(i, out var exitState))
					{
						return exitState;
					}
				}
			}

			return AllChildrenProcessedState;
		}

		protected bool EvaluateChildAt(int index, out NodeState state)
		{

			if (index < 0 || index >= Children.Count)
			{
				state = NodeState.Failure;
				return true;
			}

			var child = Children[index];

			if (child == null)
			{
				state = NodeState.Failure;
				return true;
			}

			state = child.Update();

			if (state == NodeState.Running || state == TerminalChildState)
			{
				return true;
			}

			if (index >= Children.Count - 1)
			{
				state = AllChildrenProcessedState;
				return true;
			}

			state = NodeState.Running;
			return false;
		}
	}
}
