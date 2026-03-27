using UnityEngine.Pool;

namespace Loykas.Scripting
{    
    public class OutputTriggerPool
    {
        private readonly IObjectPool<OutputTrigger> _pool;
        
        public OutputTriggerPool()
        {
            _pool = new ObjectPool<OutputTrigger>
            (
                createFunc: () => new OutputTrigger()
            );
        }

        public OutputTrigger Get()
        {
            return _pool.Get();
        }

        public void Release(OutputTrigger element)
        {
            _pool.Release(element);
        }
    }
}
