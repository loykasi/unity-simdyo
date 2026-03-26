using UnityEngine.Pool;

namespace Loykas.Scripting
{    
    public class VariablePool
    {
        private readonly IObjectPool<Variable> _pool;
        
        public VariablePool()
        {
            _pool = new ObjectPool<Variable>
            (
                createFunc: () => new Variable()
            );
        }

        public Variable Get()
        {
            return _pool.Get();
        }

        public void Release(Variable element)
        {
            _pool.Release(element);
        }
    }
}
