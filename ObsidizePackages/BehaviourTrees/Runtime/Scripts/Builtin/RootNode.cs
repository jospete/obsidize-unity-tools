using UnityEngine;

namespace Obsidize.BehaviourTrees
{

	public class RootNode : ProxyNode
	{

		[SerializeField]
		private bool _persistCompositeState = false;

		public override string PrimaryUssClass => "bt-root";
		public bool PersistCompositeState => _persistCompositeState;

		protected override NodeState OnUpdate()
		{
			return Child != null ? Child.Update() : NodeState.Failure;
		}
	}
}
