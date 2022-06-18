using UnityEngine;

namespace Obsidize.BehaviourTrees
{
    public interface INode<T> where T : Component
    {
        T Blackboard { get; }
    }
}
