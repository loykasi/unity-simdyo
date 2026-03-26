using System;
using UnityEngine.Pool;

namespace Loykas.Scripting
{    
    public class NodeTaskPool
    {
        private readonly IObjectPool<NodeTask> _pool;
        
        public NodeTaskPool()
        {
            _pool = new ObjectPool<NodeTask>
            (
                createFunc: () => new NodeTask(),
                actionOnRelease: OnRelease
            );
        }

        private void OnRelease(NodeTask task)
        {
            task.Reset();
        }

        public NodeTask Get()
        {
            return _pool.Get();
        }

        public void Release(NodeTask element)
        {
            _pool.Release(element);
        }
    }
}
