using UnityEngine;

namespace Obsidize.BehaviourTrees.Samples
{
	[DisallowMultipleComponent]
    public class ActorAIState : MonoBehaviour
    {

		[SerializeField] private ActorSensor _visibilitySensor;
		[SerializeField] private WaypointRuntimeSet _waypointRuntimeSet;
		[SerializeField] private LayerMask _threatLayers;
		[SerializeField] private float _maxPatrolDelay;

		[Space]
		[Header("Debug Info")]
		[SerializeField] private Transform _threatTransform;
		[SerializeField] private int _waypointIndex = 0;
		[SerializeField] private float _selectedPatrolDelay;
		[SerializeField] private float _patrolDelayRemaining;

		private Movement _movement;

		private Vector3 CurrentPatrolDestination => _waypointRuntimeSet.GetWaypointPositionAt(_waypointIndex);
		public bool HasPetrolDelayRemaining => _patrolDelayRemaining > 0f;

		private void Start()
		{
			_movement = GetComponent<Movement>();
		}

		private void Update()
		{
			if (HasPetrolDelayRemaining)
			{
				_patrolDelayRemaining = Mathf.Max(0f, _patrolDelayRemaining - Time.deltaTime);
			}
		}

		private void OnEnable()
		{
			_visibilitySensor.OnColliderEnter += HandleThreatEnter;
			_visibilitySensor.OnColliderExit += HandleThreatExit;
		}

		private void OnDisable()
		{
			_visibilitySensor.OnColliderEnter -= HandleThreatEnter;
			_visibilitySensor.OnColliderExit -= HandleThreatExit;
		}

		private void HandleThreatEnter(Collider collider)
		{
			_threatTransform = collider.transform;
		}

		private void HandleThreatExit(Collider collider)
		{
			if (_threatTransform != null && _threatTransform.gameObject == collider.gameObject)
			{
				_threatTransform = null;
			}
		}

		public bool TryAttackThreatTarget()
		{
			return IsWithinReachOfThreat();
		}

		public bool IsWithinReachOfThreat()
		{
			return _movement.IsWithinReachOf(_threatTransform);
		}

		public bool TryMoveTowardsThreat()
		{

			var hasThreat = HasVisibleThreat();

			if (hasThreat)
			{
				_movement.Destination = _threatTransform.position;
			}

			return hasThreat;
		}

		public bool TryWaitForPatrolDelay()
		{
			return !HasPetrolDelayRemaining;
		}

		public void StartPetrolDelay()
		{
			_selectedPatrolDelay = Random.value * _maxPatrolDelay;
			_patrolDelayRemaining = _selectedPatrolDelay;
		}

		public bool HasVisibleThreat()
		{
			return _threatTransform != null;
		}

		public bool IsMovementDestinationSetToPatrol()
		{
			return Vector3.Distance(_movement.Destination, CurrentPatrolDestination) <= Mathf.Epsilon;
		}

		public bool HasReachedPatrolDestination()
		{
			return IsMovementDestinationSetToPatrol() && _movement.HasReachedDestination;
		}

		public bool TryMoveToPatrolDestination()
		{
			_movement.Destination = CurrentPatrolDestination;
			return _movement.HasReachedDestination;
		}

		public void AdvancePatrolDestination()
		{
			
			var count = _waypointRuntimeSet.Waypoints.Count;

			if (count > 0)
			{
				_waypointIndex = (_waypointIndex + 1) % count;
			}
		}

		public void OnPatrolActionComplete()
		{
			StartPetrolDelay();
			AdvancePatrolDestination();
		}
	}
}
