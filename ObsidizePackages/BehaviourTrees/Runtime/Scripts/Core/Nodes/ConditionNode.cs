using UnityEngine;

namespace Obsidize.BehaviourTrees
{

	public abstract class ConditionNode<T> : ConditionNode, INode<T> where T : Component
	{
		public T ControllerState { get; private set; }

		public override void OnTreeAwake(BehaviourTreeController tree)
		{
			base.OnTreeAwake(tree);
			ControllerState = tree.GetComponent<T>();
		}
	}

	public abstract class ConditionNode : ActionNode
    {

		public override string PrimaryUssClass => "bt-condition";
		public abstract bool IsTrue();

		protected override NodeState OnUpdate()
		{
			return IsTrue() ? NodeState.Success : NodeState.Failure;
		}
	}
}
