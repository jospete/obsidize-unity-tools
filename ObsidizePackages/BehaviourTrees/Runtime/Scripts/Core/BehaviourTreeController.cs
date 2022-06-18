using System.Collections.Generic;
using UnityEngine;

namespace Obsidize.BehaviourTrees
{
    public class BehaviourTreeController : MonoBehaviour
    {

        [SerializeField]
        private BehaviourTree _tree;

        private BehaviourTree _treeInstance;

        public BehaviourTree TreePrefab => _tree;
        public BehaviourTree ActiveTree => _treeInstance;

		protected virtual void Awake()
		{
			_treeInstance = _tree.Clone();
            _treeInstance.Root.OnTreeAwake(this);
		}

        protected virtual void Update()
        {
            _treeInstance.Update();
        }
    }
}
