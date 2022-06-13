namespace Obsidize.BehaviourTrees.Samples
{

	public class NoVisibleThreat : ConditionNode<ActorAIState>
	{
		public override bool IsTrue()
		{
			return !ControllerState.HasVisibleThreat();
		}
	}
}
