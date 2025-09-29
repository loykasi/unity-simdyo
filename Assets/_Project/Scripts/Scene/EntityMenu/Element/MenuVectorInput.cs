using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Loykas.Scripting;

public class MenuVectorInput : MonoBehaviour
{
    public event UnityAction<Vector3> OnSubmit;

    private Vector3 _value;

    [SerializeField] private string _name;
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private NumberInput _xInput;
    [SerializeField] private NumberInput _yInput;

    private void OnValidate()
    {
        _label.text = _name;
    }

    private void Awake()
    {
        _xInput.OnSubmit += OnXSubmit;
        _yInput.OnSubmit += OnYSubmit;
    }

    private void OnXSubmit(object value)
    {
        _value.x = (float)value;
        OnSubmit?.Invoke(_value);
    }

    private void OnYSubmit(object value)
    {
        _value.y = (float)value;
        OnSubmit?.Invoke(_value);
    }

    public void SetValue(Vector3 value)
    {
        _value = value;
        _xInput.SetValue(value.x);
        _yInput.SetValue(value.y);
    }
}