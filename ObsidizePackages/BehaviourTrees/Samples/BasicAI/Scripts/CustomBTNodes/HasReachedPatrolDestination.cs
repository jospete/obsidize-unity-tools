namespace Obsidize.BehaviourTrees.Samples
{
	public class HasReachedPatrolDestination : ConditionNode<ActorAIState>
	{

		public override bool IsTrue()
		{
			return ControllerState.HasReachedPatrolDestination();
		}

		protected override void OnStop()
		{
			if (DidSucceed) ControllerState.OnPatrolActionComplete();
		}
	}
}
