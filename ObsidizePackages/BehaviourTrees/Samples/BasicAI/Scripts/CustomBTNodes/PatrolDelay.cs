namespace Obsidize.BehaviourTrees.Samples
{
	public class PatrolDelay : ActionNode<ActorAIState>
	{

		protected override void OnStart()
		{
			base.OnStart();
			ControllerState.OnStartPetrolDelay();
		}

		protected override NodeState OnUpdate()
		{
			return ControllerState.IsWaitingForPatrolDelay()
				? NodeState.Running
				: NodeState.Success;
		}
	}
}
