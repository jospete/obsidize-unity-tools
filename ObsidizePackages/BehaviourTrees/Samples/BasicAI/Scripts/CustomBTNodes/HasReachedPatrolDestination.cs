namespace Obsidize.BehaviourTrees.Samples
{
	public class HasReachedPatrolDestination : ConditionNode<ActorAIState>
	{

		public override bool IsTrue()
		{
			return Blackboard.HasReachedPatrolDestination();
		}

		protected override void OnStop()
		{
			if (DidSucceed) Blackboard.OnPatrolActionComplete();
		}
	}
}
