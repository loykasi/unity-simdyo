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
    public InputTrigger Enter;
    public OutputTrigger Exit;

    public InputValue Input;

    public SetPositionNode(string title) : base(title)
    {
        Enter = InputTrigger(nameof(Enter), Set);
        Exit = OutputTrigger(nameof(Exit));

        Input = InputValue(nameof(Input));
    }

    public OutputTrigger Set(ScriptFlow vs)
    {
        Vector3 value = (Vector3)Input.GetValue(vs);
        vs.Entity.transform.position = value;
        return Exit;
    }
}