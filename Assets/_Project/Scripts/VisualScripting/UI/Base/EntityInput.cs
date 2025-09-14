using System;
using UnityEngine.UI;

public class EntityInput : BaseInput
{
    public Dropdown Dropdown;

    private void Awake()
    {
        Dropdown.onValueChanged.AddListener(OnValueChanged);

        var options = ObjectManager.Instance.GetEntityOptions();
        Dropdown.AddOptions(options);
    }

    private void OnValueChanged(int arg0)
    {
        throw new NotImplementedException();
    }

    public override object GetValue()
    {
        return null;
    }
}