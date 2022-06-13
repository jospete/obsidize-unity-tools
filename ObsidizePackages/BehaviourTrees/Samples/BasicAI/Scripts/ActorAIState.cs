using UnityEngine;

namespace Obsidize.BehaviourTrees.Samples
{
	[DisallowMultipleComponent]
    public class ActorAIState : MonoBehaviour
    {

		[SerializeField] private ActorSensor _visibilitySensor;
		[SerializeField] private Transform[] _waypoints;
		[SerializeField] private LayerMask _threatLayers;
		[SerializeField] private float _maxPatrolDelay;

		[Space]
		[Header("Debug Info")]
		[SerializeField] private Transform _threatTransform;
		[SerializeField] private int _waypointIndex = 0;
		[SerializeField] private float _selectedPatrolDelay;
		[SerializeField] private float _patrolDelayRemaining;

		private Movement _movement;

		private void Start()
		{
			_movement = GetComponent<Movement>();
		}

		private void Update()
		{
			if (IsWaitingForPatrolDelay())
			{
				_patrolDelayRemaining = Mathf.Max(0f, _patrolDelayRemaining - Time.deltaTime);
			}
		}

		private void OnEnable()
		{
			_visibilitySensor.OnColliderEnter += HandleThreatEnter;
		}

		private void OnDisable()
		{
			_visibilitySensor.OnColliderEnter -= HandleThreatEnter;
		}

		private void HandleThreatEnter(Collider collider)
		{
			_threatTransform = collider.transform;
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

		public bool IsWaitingForPatrolDelay()
		{
			return _patrolDelayRemaining > 0f;
		}

		public void OnStartPetrolDelay()
		{
			_selectedPatrolDelay = Random.value * _maxPatrolDelay;
			_patrolDelayRemaining = _selectedPatrolDelay;
		}

		public bool HasVisibleThreat()
		{
			return _threatTransform != null;
		}

		public void OnStartPatrol()
		{
			_waypointIndex = (_waypointIndex + 1) % _waypoints.Length;
			_movement.Destination = _waypoints[_waypointIndex].position;
		}

		public bool IsMovingToPatrolDestination()
		{
			return !_movement.HasReachedDestination;
		}
	}
}
