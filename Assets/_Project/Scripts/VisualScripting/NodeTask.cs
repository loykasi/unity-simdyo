namespace Loykas.Scripting
{
    public class NodeTask
    {
        public OutputTrigger From;
        public InputTrigger Trigger;

        public void Invoke(ScriptFlow flow)
        {
            if (Trigger.Invoke(flow))
            {
                Trigger = Trigger.TargetOutputTrigger.Invoke(flow);
                if (Trigger != null)
                {
                    Invoke(flow);
                }
                else
                {
                    flow.RemoveTask(this);
                }
            }
        }
    }
}