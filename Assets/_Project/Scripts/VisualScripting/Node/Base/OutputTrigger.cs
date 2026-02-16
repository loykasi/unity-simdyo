namespace Loykas.Scripting
{
    public class OutputTrigger : Port<InputTrigger>
    {
        public string Name;
        public InputTrigger Destination;

        public override bool ShouldShowLabel { get; set; } = false;

        public OutputTrigger(string key) : base(key)
        {
        }

        public OutputTrigger NoLocalize()
        {
            ShouldLocalized = false;
            return this;
        }

        public OutputTrigger ShowLabel()
        {
            ShouldShowLabel = true;
            return this;
        }

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
    }
}