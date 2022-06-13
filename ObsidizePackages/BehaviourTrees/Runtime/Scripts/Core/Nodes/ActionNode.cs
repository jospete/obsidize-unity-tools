using UnityEngine;

namespace Obsidize.BehaviourTrees
{

	public abstract class ActionNode<T> : ActionNode, INode<T> where T : Component
	{
		public T ControllerState { get; private set; }

		public override void OnTreeAwake(BehaviourTreeController tree)
		{
			base.OnTreeAwake(tree);
			ControllerState = tree.GetComponent<T>();
		}
	}

    public abstract class ActionNode : Node
    {

		public override string PrimaryUssClass => "bt-action";
		public sealed override NodeChildCapacity ChildCapacity => NodeChildCapacity.None;
	}
}
