using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DynamicVariable", menuName = "Scriptable Objects/Visual Scripting/Node/Dynamic Variable")]
public class DynamicVariableNodeData : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new DynamicVariableNode(Title);
    }
}