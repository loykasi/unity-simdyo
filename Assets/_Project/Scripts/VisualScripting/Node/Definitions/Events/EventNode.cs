namespace Loykas.Scripting
{
    public abstract class EventNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Event;

        public OutputTrigger Exit;
        public abstract EventHook Hook { get; }

        public EventNode()
        {
            Exit = CreateOutputTrigger(nameof(Exit), PortSettings.Default);
        }

        public void Register(ScriptFlow vs)
        {
            vs.RegisterEventNode(Hook, this);
        }

        public virtual bool ShouldTrigger()
        {
            return true;
        }

        public virtual void AssignArgument(object args)
        {

        }
    }
}