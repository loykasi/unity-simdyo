using UnityEngine;

[CreateAssetMenu(fileName = "MakeNumber", menuName = "Scriptable Objects/Visual Scripting/Node/Make Number")]
public class MakeNumber : MakeVariable
{
    public override Variable Type => Variable.Number;
}