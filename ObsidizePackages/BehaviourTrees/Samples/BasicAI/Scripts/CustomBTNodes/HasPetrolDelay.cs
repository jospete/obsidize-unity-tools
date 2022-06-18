namespace Obsidize.BehaviourTrees.Samples
{
	public class HasPetrolDelay : ConditionNode<ActorAIState>
	{
		public override bool IsTrue()
		{
			return Blackboard.HasPetrolDelayRemaining;
		}
	}
}
