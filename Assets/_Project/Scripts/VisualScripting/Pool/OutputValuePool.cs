using UnityEngine.Pool;

namespace Loykas.Scripting
{    
    public class OutputValuePool
    {
        private readonly IObjectPool<OutputValue> _pool;
        
        public OutputValuePool()
        {
            _pool = new ObjectPool<OutputValue>
            (
                createFunc: () => new OutputValue()
            );
        }

        public OutputValue Get()
        {
            return _pool.Get();
        }

        public void Release(OutputValue element)
        {
            _pool.Release(element);
        }
    }
}
