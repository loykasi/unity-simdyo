namespace Loykas.Scripting
{
    public class OutputTrigger : Port<InputTrigger>
    {
        public string Name;
        public InputTrigger Destination;

        public OutputTrigger(string key) : base(key)
        {
        }

        public OutputTrigger NoLocalize()
        {
            ShouldLocalized = false;
            return this;
        }

        public OutputTrigger HideLabel()
        {
            ShouldShowLabel = false;
            return this;
        }

        public override bool CanConnectTo(InputTrigger port)
        {
            return true;
        }

        public override void Connect(InputTrigger port)
        {
            DisconnectPort(Destination);
            Destination = port;
        }

        public InputTrigger Invoke(ScriptFlow vs)
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