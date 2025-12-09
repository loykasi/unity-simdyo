namespace Loykas.Scripting
{
    public class UIInputTriggerPort : UINodePort
    {
        public override NodePortEdge Edge => NodePortEdge.Left;
        public override NodePortType Type => NodePortType.Trigger;
    }
}