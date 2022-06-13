using UnityEngine;

namespace Obsidize.BehaviourTrees
{

    public class DebugLogNode : ActionNode
    {

        public string message;

		private void LogCallback(string callbackName)
		{
			Debug.Log($"[{callbackName}] {message}");
		}

		protected override void OnStart()
		{
			LogCallback(nameof(OnStart));
		}

		protected override void OnStop()
		{
			LogCallback(nameof(OnStop));
		}

		protected override NodeState OnUpdate()
		{
			LogCallback(nameof(OnUpdate));
			return NodeState.Success;
		}
	}
}
