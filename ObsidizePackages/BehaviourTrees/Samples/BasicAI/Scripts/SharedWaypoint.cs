using UnityEngine;

namespace Obsidize.BehaviourTrees.Samples
{
    [DisallowMultipleComponent]
    public class SharedWaypoint : MonoBehaviour
    {

        [SerializeField] private WaypointRuntimeSet _waypointSet;
		[SerializeField] private int _waypointIndex;

		public int WaypointIndex => _waypointIndex;

		private void OnEnable()
		{
			_waypointSet.Add(this);
		}

		private void OnDisable()
		{
			_waypointSet.Remove(this);
		}
	}
}
