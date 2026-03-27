using UnityEngine.Pool;

namespace Loykas.Scripting
{    
    public class InputTriggerPool
    {
        private readonly IObjectPool<InputTrigger> _pool;
        
        public InputTriggerPool()
        {
            _pool = new ObjectPool<InputTrigger>
            (
                createFunc: () => new InputTrigger()
            );
        }

        public InputTrigger Get()
        {
            return _pool.Get();
        }

        public void Release(InputTrigger element)
        {
            _pool.Release(element);
        }
    }
}
