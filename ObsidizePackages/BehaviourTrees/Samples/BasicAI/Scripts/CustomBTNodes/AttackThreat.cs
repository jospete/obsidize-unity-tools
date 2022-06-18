namespace Obsidize.BehaviourTrees.Samples
{
    public class AttackThreat : ActionNode<ActorAIState>
    {
		protected override NodeState OnUpdate()
		{
			return Blackboard.TryAttackThreatTarget()
				? NodeState.Success
				: NodeState.Failure;
		}
    }
}
