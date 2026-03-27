namespace Loykas.Scripting
{
    public class StartNode : EventNode
    {
        public override ScriptNode Create()
        {
            return new StartNode();
        }

        public override EventHook Hook => EventHook.Start;
    }
}