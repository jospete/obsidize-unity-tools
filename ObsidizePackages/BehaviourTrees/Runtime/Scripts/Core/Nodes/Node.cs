using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obsidize.BehaviourTrees
{

    public abstract class Node : ScriptableObject
    {

        [Space]
        [TextArea]
        [SerializeField]
        private string _description;

        [SerializeField]
        [HideInInspector]
        private string _guid;

        [SerializeField]
        [HideInInspector]
        private Vector2 _graphPosition;

        public virtual string DisplayName => GetType().Name;
        public string Description => _description;
        public virtual string PrimaryUssClass => "bt-node";
        public abstract NodeChildCapacity ChildCapacity { get; }

        public NodeState State { get; protected set; } = NodeState.Running;
        public bool Started { get; protected set; }
        public BehaviourTreeController Controller { get; protected set; }
        public bool Idle => State == NodeState.Running && !Started;
        public bool DidSucceed => Started && State == NodeState.Success;
        public bool DidFail => Started && State == NodeState.Failure;

        public string Guid
        {
            get => _guid;
            set => _guid = value;
        }

		public Vector2 GraphPosition
        {
            get => _graphPosition;
            set => _graphPosition = value;
        }

        protected abstract NodeState OnUpdate();

        protected virtual void OnStart()
        {
        }

        protected virtual void OnStop()
        {
        }

        public virtual bool AttachChild(Node child)
		{
            return false;
		}

        public virtual bool DetachChild(Node node)
		{
            return false;
		}

        public virtual List<Node> GetChildren()
		{
            return null;
        }

        public virtual void DetachAllChildren()
        {
        }

        public virtual Node Clone()
		{
            return Instantiate(this);
        }

        public NodeState Update()
		{

            if (!Started)
			{
                OnStart();
                Started = true;
            }

            State = OnUpdate();

            if (State == NodeState.Failure || State == NodeState.Success)
			{
                OnStop();
                Started = false;
            }

            return State;
        }

        public virtual void OnTreeAwake(BehaviourTreeController controller)
        {

            Controller = controller;

            var children = GetChildren();

            if (children == null || children.Count <= 0)
            {
                return;
            }

            foreach (var child in children)
            {
                if (child != null)
                {
                    child.OnTreeAwake(controller);
                }
            }
        }
	}
}
