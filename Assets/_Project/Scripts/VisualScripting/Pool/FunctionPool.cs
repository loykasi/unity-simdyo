using UnityEngine.Pool;

namespace Loykas.Scripting
{    
    public class FunctionPool
    {
        private readonly IObjectPool<ScriptFunction> _functionPool;
        private readonly IObjectPool<FunctionInput> _inputPool;
        
        public FunctionPool()
        {
            _functionPool = new ObjectPool<ScriptFunction>
            (
                createFunc: () => new ScriptFunction()
            );

            _inputPool = new ObjectPool<FunctionInput>
            (
                createFunc: () => new FunctionInput()
            );
        }

        public ScriptFunction GetFunction()
        {
            return _functionPool.Get();
        }

        public FunctionInput GetInput()
        {
            return _inputPool.Get();
        }

        public void Release(ScriptFunction element)
        {
            _functionPool.Release(element);
        }

        public void Release(FunctionInput element)
        {
            _inputPool.Release(element);
        }
    }
}
