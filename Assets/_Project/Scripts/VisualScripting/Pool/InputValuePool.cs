using UnityEngine.Pool;

namespace Loykas.Scripting
{    
    public class InputValuePool
    {
        private readonly IObjectPool<InputValue> _pool;
        
        public InputValuePool()
        {
            _pool = new ObjectPool<InputValue>
            (
                createFunc: () => new InputValue()
            );
        }

        public InputValue Get()
        {
            return _pool.Get();
        }

        public void Release(InputValue element)
        {
            _pool.Release(element);
        }
    }
}
