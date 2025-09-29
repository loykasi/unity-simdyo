using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Loykas.Scripting;

public class MenuNumberInput : MonoBehaviour
{
    public event UnityAction<float> OnSubmit;

    [SerializeField] private string _name;
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private NumberInput _input;

    private void OnValidate()
    {
        _label.text = _name;
    }

    private void Awake()
    {
        _input.OnSubmit += OnInputSubmit;
    }

    private void OnInputSubmit(object value)
    {
        OnSubmit?.Invoke((float)value);
    }

    public void SetValue(float value)
    {
        _input.SetValue(value);
    }
}