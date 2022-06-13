using System;
using UnityEngine;

namespace Obsidize.BehaviourTrees.Samples
{
    [DisallowMultipleComponent]
    public class ActorSensor : MonoBehaviour
    {

		public event Action<Collider> OnColliderEnter;
		public event Action<Collider> OnColliderExit;

		[SerializeField] private LayerMask _layers;

		private void OnTriggerEnter(Collider other)
		{
			CheckInvoke(other, OnColliderEnter);
		}

		private void OnTriggerExit(Collider other)
		{
			CheckInvoke(other, OnColliderExit);
		}

		private void CheckInvoke(Collider other, Action<Collider> callback)
		{
			if (callback != null && other != null && _layers.Includes(other.gameObject.layer))
			{
				callback.Invoke(other);
			}
		}
	}
}
