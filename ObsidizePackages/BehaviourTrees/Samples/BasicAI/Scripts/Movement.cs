using UnityEngine;

namespace Obsidize.BehaviourTrees.Samples
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public class Movement : MonoBehaviour
    {

        [SerializeField] private Vector3 _destination;
        [SerializeField] private float _stoppingDistance = 0.5f;
        [SerializeField] private float _moveSpeed = 10f;

        public Vector3 Destination
		{
            get => _destination;
            set => _destination = value;
		}

        public float StoppingDistance
		{
            get => _stoppingDistance;
            set => _stoppingDistance = value;
		}

        public float MoveSpeed
		{
            get => _moveSpeed;
            set => _moveSpeed = value;
		}

        public Vector3 Position => transform.position;
        public float DestinationDelta => Vector3.Distance(Position, Destination);
        public bool HasReachedDestination => IsWithinReachOf(Destination);

        public bool IsWithinReachOf(Vector3 position)
		{
            return Vector3.Distance(Position, position) <= StoppingDistance;
		}

		public bool IsWithinReachOf(Transform t)
		{
            return t != null && IsWithinReachOf(t.position);
        }

		private Rigidbody _physicsBody;

		private void Start()
		{
            _physicsBody = GetComponent<Rigidbody>();
        }

		private void Update()
		{
            
            if (HasReachedDestination)
			{
                _physicsBody.velocity = Vector3.zero;
                return;
			}

            var destination = Destination;
            destination.y = Position.y;

            transform.LookAt(destination);
            _physicsBody.velocity = transform.forward * _moveSpeed;
        }
	}
}
