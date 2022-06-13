namespace Obsidize.BehaviourTrees.Samples
{
    public class PatrolAction : ActionNode<ActorAIState>
    {

		protected override void OnStart()
		{
			base.OnStart();
			ControllerState.OnStartPatrol();
		}

		protected override NodeState OnUpdate()
		{
			return ControllerState.IsMovingToPatrolDestination()
				? NodeState.Running
				: NodeState.Success;
		}
    }
}
