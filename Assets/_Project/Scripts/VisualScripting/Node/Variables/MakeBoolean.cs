using UnityEngine;

[CreateAssetMenu(fileName = "MakeBoolean", menuName = "Scriptable Objects/Visual Scripting/Node/Make Boolean")]
public class MakeBoolen : MakeVariable
{
    public override Variable Type => Variable.Boolean;
}