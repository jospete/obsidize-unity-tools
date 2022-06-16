using System.Collections.Generic;
using UnityEngine;

namespace Obsidize.BehaviourTrees
{
    public abstract class ProxyNode : Node
    {

		[SerializeField]
		[HideInInspector]
		private Node _child;

		public sealed override NodeChildCapacity ChildCapacity => NodeChildCapacity.Single;

		public Node Child
		{
			get => _child;
			set => _child = value;
		}

		public override Node Clone()
		{
			var result = Instantiate(this);
			result._child = _child != null ? _child.Clone() : null;
			return result;
		}

		public override List<Node> GetChildren()
		{
			return new List<Node> { Child };
		}

		public override void DetachAllChildren()
		{
			Child = null;
		}

		public override bool AttachChild(Node child)
		{
			Child = child;
			return true;
		}

		public override bool DetachChild(Node node)
		{

			var canRemove = Child == node;

			if (canRemove)
			{
				Child = null;
			}

			return canRemove;
		}
	}
}
