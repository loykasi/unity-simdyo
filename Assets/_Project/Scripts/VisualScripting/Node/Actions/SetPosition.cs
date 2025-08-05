using UnityEngine;

[CreateAssetMenu(fileName = "SetPosition", menuName = "Scriptable Objects/Visual Scripting/Node/Set Position")]
public class SetPosition : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new SetPositionNode(Title);
    }
}

class SetPositionNode : ScriptNode
{
    public InputTrigger Trigger;
    public OutputTrigger Triggered;

    public ValueInput Input;

    public SetPositionNode(string title) : base(title)
    {
        Trigger = CreateInputTrigger(Set);
        Triggered = CreateOutputTrigger();

        Input = ValueInput(DataType.Vector, true);
    }

    public OutputTrigger Set(VisualScripting vs)
    {
        Vector3 value = (Vector3)Input.GetValue(vs);
        vs.Entity.transform.position = value;
        return Triggered;
    }
}