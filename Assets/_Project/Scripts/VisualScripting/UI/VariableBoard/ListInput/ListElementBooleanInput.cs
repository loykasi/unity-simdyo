using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ListElementBooleanInput: ListElementInput
{
    public Toggle Toggle;
    public Button RemoveButton;

    public override object DefaultValue => false;

    private void Awake()
    {
        Toggle.onValueChanged.AddListener(EndEdit);
        RemoveButton.onClick.AddListener(Remove);
    }

    private void Remove()
    {
        OnRemove?.Invoke(this);
    }

    private void EndEdit(bool value)
    {
        OnEndEdit?.Invoke(this, value);
    }

    public override void SetValue(object value)
    {
        Toggle.SetIsOnWithoutNotify((bool)value);
    }
}