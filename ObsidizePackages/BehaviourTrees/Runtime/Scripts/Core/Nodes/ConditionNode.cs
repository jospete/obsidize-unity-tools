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

		[SerializeField] private bool _invertInput;

		public bool InvertInput => _invertInput;
		protected string DisplayNamePrefix => InvertInput ? "(!) " : string.Empty;
		public override string DisplayName => DisplayNamePrefix + base.DisplayName;
		public override string PrimaryUssClass => "bt-condition";
		public abstract bool IsTrue();

		protected bool HasExpectedInput()
		{
			return _invertInput != IsTrue();
		}

		protected override NodeState OnUpdate()
		{
			return HasExpectedInput() 
				? NodeState.Success 
				: NodeState.Failure;
		}
	}
}
