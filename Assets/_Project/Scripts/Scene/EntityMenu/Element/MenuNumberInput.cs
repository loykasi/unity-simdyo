using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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

    private void OnInputSubmit(float value)
    {
        OnSubmit?.Invoke(value);
    }

    public void SetValue(float value)
    {
        _input.SetValue(value);
    }
}