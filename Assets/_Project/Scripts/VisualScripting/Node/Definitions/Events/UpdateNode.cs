namespace Loykas.Scripting
{
    public class UpdateNode : EventNode
    {
        public override ScriptNode Create()
        {
            return new UpdateNode();
        }

        public override EventHook Hook => EventHook.Update;
    }
}