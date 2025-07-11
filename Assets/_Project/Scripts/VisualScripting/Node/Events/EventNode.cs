public abstract class EventNode : ScriptNode
{
    public OutputTrigger outputTrigger;

    public EventNode(string title) : base(title)
    {
        outputTrigger = CreateOutputTrigger();
    }

    public abstract EventHook GetHook();
}