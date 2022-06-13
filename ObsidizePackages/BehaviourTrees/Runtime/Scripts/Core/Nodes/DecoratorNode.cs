using UnityEngine;

namespace Obsidize.BehaviourTrees
{

	public abstract class DecoratorNode<T> : DecoratorNode, INode<T> where T : Component
	{
		public T ControllerState { get; private set; }

		public override void OnTreeAwake(BehaviourTreeController tree)
		{
			base.OnTreeAwake(tree);
			ControllerState = tree.GetComponent<T>();
		}
	}


	public abstract class DecoratorNode : ProxyNode
	{

		public override string PrimaryUssClass => "bt-decorator";
	}
}
