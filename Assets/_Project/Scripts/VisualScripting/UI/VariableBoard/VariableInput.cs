using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class VariableInput : MonoBehaviour, IVariableInput
{
    public abstract DataType Type { get; }
    public UnityAction OnValueUpdated;
    public VariableBoardItem VariableItem;

    public float Height { get; protected set; } = 30f;

    public abstract void Disable();
    public abstract void Enable();
    public abstract object GetValue();
    public abstract void SetValue(object value);
    // public abstract float GetHeight();
}