namespace Obsidize.BehaviourTrees
{

	public class RepeatNode : DecoratorNode
	{

		protected override NodeState OnUpdate()
		{

			if (Child != null)
			{
				Child.Update();
			}

			return NodeState.Running;
		}
	}
}
