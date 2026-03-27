namespace Loykas.Scripting
{
    public class StartAsCloneNode : EventNode
    {
        public override bool CanUseGlobal => false;

        public override ScriptNode Create()
        {
            return new StartAsCloneNode();
        }

        public override EventHook Hook => EventHook.StartAsClone;
    }
}