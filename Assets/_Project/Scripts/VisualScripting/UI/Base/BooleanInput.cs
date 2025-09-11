using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BooleanInput : MonoBehaviour
{
    public event UnityAction<bool> OnSubmit;

    public RectTransform Rect;
    public Toggle Input;

    private void Awake()
    {
        Input.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool value)
    {
        OnSubmit?.Invoke(value);
    }

    public void SetValue(bool value)
    {
        Input.SetIsOnWithoutNotify(value);
    }

    public bool GetValue()
    {
        return Input.isOn;
    }
}