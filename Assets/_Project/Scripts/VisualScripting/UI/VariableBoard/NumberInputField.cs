using TMPro;
using UnityEngine;

public class NumberInputField : VariableInput
{
    [SerializeField] private TMP_InputField _valueInputField;
    private float _value;

    private void Awake()
    {
        _valueInputField.onEndEdit.AddListener(OnValueChanged);
    }

    private void OnValueChanged(string value)
    {
        if (float.TryParse(value, out float result))
        {
            _valueInputField.SetTextWithoutNotify(result.ToString());
            _value = result;
        }
        else
        {
            _valueInputField.SetTextWithoutNotify("0");
            _value = 0.0f;
        }
        OnValueUpdated?.Invoke();
    }

    public override void SetValue(object value)
    {
        _value = (float)value;
        _valueInputField.text = value.ToString();
    }

    public override void Enable()
    {
        gameObject.SetActive(true);
        _valueInputField.text = "0";
    }

    public override void Disable()
    {
        gameObject.SetActive(false);
    }

    public override object GetValue()
    {
        return _value;
    }
}