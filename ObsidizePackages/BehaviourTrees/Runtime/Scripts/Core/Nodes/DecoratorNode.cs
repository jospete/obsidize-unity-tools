using UnityEngine;

namespace Obsidize.BehaviourTrees
{

	public abstract class DecoratorNode<T> : DecoratorNode, INode<T> where T : Component
	{
		public T Blackboard { get; private set; }

		public override void OnTreeAwake(BehaviourTreeController tree)
		{
			base.OnTreeAwake(tree);
			Blackboard = tree.GetComponent<T>();
		}
	}


	public abstract class DecoratorNode : ProxyNode
	{

		public override string PrimaryUssClass => "bt-decorator";
	}
}
