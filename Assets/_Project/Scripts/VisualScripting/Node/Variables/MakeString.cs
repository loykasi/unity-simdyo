using UnityEngine;

[CreateAssetMenu(fileName = "MakeString", menuName = "Scriptable Objects/Visual Scripting/Node/Make String")]
public class MakeString : MakeVariable
{
    public override Variable Type => Variable.String;
}