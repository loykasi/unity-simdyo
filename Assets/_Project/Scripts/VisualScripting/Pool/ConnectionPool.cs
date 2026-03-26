using UnityEngine.Pool;

namespace Loykas.Scripting
{    
    public class ConnectionPool
    {
        private readonly IObjectPool<NodeConnection> _pool;
        
        public ConnectionPool()
        {
            _pool = new ObjectPool<NodeConnection>
            (
                createFunc: () => new NodeConnection()
            );
        }

        public NodeConnection Get()
        {
            return _pool.Get();
        }

        public void Release(NodeConnection element)
        {
            _pool.Release(element);
        }
    }
}
