using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ListElementNumberInputField : ListElementInput
{
    public override object DefaultValue => 0f;

    public TMP_InputField InputField;
    public Button RemoveButton;

    private float _value = 0f;

    private void Awake()
    {
        InputField.onEndEdit.AddListener(EndEdit);
        RemoveButton.onClick.AddListener(Remove);
    }

    private void Remove()
    {
        OnRemove?.Invoke(this);
    }

    private void EndEdit(string value)
    {
        if (float.TryParse(value, out float result))
        {
            InputField.text = result.ToString();
            _value = result;
        }
        else
        {
            InputField.text = _value.ToString();
        }
        OnEndEdit?.Invoke(this, _value);
    }

    public override void SetValue(object value)
    {
        InputField.SetTextWithoutNotify(value.ToString());
    }
}