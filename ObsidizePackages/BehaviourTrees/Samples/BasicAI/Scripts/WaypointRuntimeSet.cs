using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obsidize.BehaviourTrees.Samples
{
    [CreateAssetMenu]
    public class WaypointRuntimeSet : ScriptableObject
    {

        private List<SharedWaypoint> _waypoints = new List<SharedWaypoint>();

        public IReadOnlyCollection<SharedWaypoint> Waypoints => _waypoints;

		public Vector3 GetWaypointPositionAt(int waypointIndex)
		{
			return waypointIndex >= 0 && waypointIndex < _waypoints.Count
				? _waypoints[waypointIndex].transform.position
				: Vector3.zero;
		}

		public void Add(SharedWaypoint waypoint)
		{
            if (waypoint != null && !_waypoints.Contains(waypoint))
			{
                _waypoints.Add(waypoint);
                SortWaypoints();
			}
		}

		public void Remove(SharedWaypoint waypoint)
		{
            if (_waypoints.Remove(waypoint))
			{
				SortWaypoints();
			}
		}

		private void SortWaypoints()
		{
			_waypoints.Sort(SortByIndex);
		}

		private int SortByIndex(SharedWaypoint x, SharedWaypoint y)
		{
			return x.WaypointIndex.CompareTo(y.WaypointIndex);
		}
	}
}
