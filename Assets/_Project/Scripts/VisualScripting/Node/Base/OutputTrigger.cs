namespace Loykas.Scripting
{
    public class OutputTrigger : Port<InputTrigger>
    {
        public string Name;
        public InputTrigger Destination;

        public override bool CanConnectTo(InputTrigger port)
        {
            return true;
        }

        public override void Connect(InputTrigger port)
        {
            if (Destination != null)
            {
                Node.Flow.Disconnect(this, Destination);
            }
            // DisconnectPort(Destination);
            Destination = port;
        }

        public InputTrigger Invoke()
        {
            return Destination;
        }

        protected override void DisconnectPort(InputTrigger port)
        {
            if (Destination != port)
            {
                return;
            }
            Destination = null;
        }

        public override void Release()
        {
            base.Release();
            Destination = null;
            
            ScriptPool.Instance.OutputTrigger.Release(this);
        }
    }
}