using TMPro;
using UnityEngine;

public class VectorInputField : VariableInput
{
    public override DataType Type => DataType.Vector;

    [SerializeField] private TMP_InputField _xInputField;
    [SerializeField] private TMP_InputField _yInputField;
    private Vector3 _value;

    private void Awake()
    {
        _xInputField.onEndEdit.AddListener(OnXChanged);
        _yInputField.onEndEdit.AddListener(OnYChanged);
    }

    private void OnXChanged(string value)
    {
        if (float.TryParse(value, out float result))
        {
            _xInputField.SetTextWithoutNotify(result.ToString());
            _value = new Vector3(result, _value.y);
        }
        else
        {
            _xInputField.SetTextWithoutNotify("0");
            _value = Vector3.zero;
        }
        OnValueUpdated?.Invoke();
    }

    private void OnYChanged(string value)
    {
        if (float.TryParse(value, out float result))
        {
            _yInputField.SetTextWithoutNotify(result.ToString());
            _value = new Vector3(_value.x, result);
        }
        else
        {
            _yInputField.SetTextWithoutNotify("0");
            _value = Vector3.zero;
        }
        OnValueUpdated?.Invoke();
    }

    public override void SetValue(object value)
    {
        _value = (Vector3)value;
        _xInputField.text = _value.x.ToString();
        _yInputField.text = _value.y.ToString();
    }

    public override void Enable()
    {
        gameObject.SetActive(true);
        _xInputField.text = "0";
        _yInputField.text = "0";
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