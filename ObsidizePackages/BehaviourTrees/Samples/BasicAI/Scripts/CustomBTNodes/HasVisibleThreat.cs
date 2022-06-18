namespace Obsidize.BehaviourTrees.Samples
{

	public class HasVisibleThreat : ConditionNode<ActorAIState>
	{
		public override bool IsTrue()
		{
			return Blackboard.HasVisibleThreat();
		}
	}
}
