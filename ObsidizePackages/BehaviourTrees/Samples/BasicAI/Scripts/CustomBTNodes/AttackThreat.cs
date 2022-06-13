namespace Obsidize.BehaviourTrees.Samples
{
    public class AttackThreat : ActionNode<ActorAIState>
    {
		protected override NodeState OnUpdate()
		{
			return ControllerState.TryAttackThreatTarget()
				? NodeState.Success
				: NodeState.Failure;
		}
    }
}
