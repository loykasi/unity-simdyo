using System;
using System.Collections.Generic;
using TMPro;

public class VariableNameInput : BaseInput
{
    public TMP_Dropdown Dropdown;

    private void Awake()
    {
        Dropdown.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(int index)
    {
        string key = Dropdown.options[index].text;

        OnSubmit?.Invoke(key);
    }

    public void Init(List<string> options)
    {
        Dropdown.ClearOptions();
        Dropdown.AddOptions(options);
    }

    public override object GetValue()
    {
        return Dropdown.options[Dropdown.value].text;
    }
}