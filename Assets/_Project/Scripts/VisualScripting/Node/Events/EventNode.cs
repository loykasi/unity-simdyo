using System.Diagnostics;
using Newtonsoft.Json;

public abstract class EventNode : ScriptNode
{
    [JsonIgnore]
    public OutputTrigger Exit;

    [JsonIgnore]
    public abstract EventHook Hook { get; }

    public EventNode(string title) : base(title)
    {
        Exit = OutputTrigger(nameof(Exit)).HideLabel();
    }

    public void Register(ScriptFlow vs)
    {
        vs.RegisterEventNode(Hook, this);
    }
}