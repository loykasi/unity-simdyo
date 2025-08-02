using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class VariableInput : MonoBehaviour, IVariableInput
{
    public UnityAction OnValueUpdated;
    
    public abstract void Disable();
    public abstract void Enable();
    public abstract object GetValue();
    public abstract void SetValue(object value);
}