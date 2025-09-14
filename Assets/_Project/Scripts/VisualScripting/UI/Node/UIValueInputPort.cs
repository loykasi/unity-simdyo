using TMPro;
using UnityEngine;

public class UIValueInputPort : UINodePort
{
    public override NodePortEdge Edge => NodePortEdge.Left;
    private InputValue _inputValue;

    [SerializeField] private StringInput _stringInput;
    [SerializeField] private NumberInput _numberInput;
    [SerializeField] private BooleanInput _booleanInput;
    [SerializeField] private TMP_Dropdown _dropdown;

    private float _height = 30f;
    private readonly float _handleSize = 30f;
    private readonly float _inputOffset = 10f;

    public override void Init()
    {
        base.Init();

        if (Port is not InputValue)
        {
            Debug.LogError("Wrong port assignment.", this);
            return;
        }
        _inputValue = (InputValue)Port;

        // _stringInput.OnSubmit += OnStringInputSubmit;
        _stringInput.OnValueUpdated += OnStringInputValueUpdated;
        
        // _numberInput.OnSubmit += OnNumberInputSubmit;
        _numberInput.OnValueUpdated += OnNumberInputValueUpdated;

        // _booleanInput.OnSubmit += OnBooleanInputSubmit;

        _dropdown.onValueChanged.AddListener(OnDropDownValueChanged);

        HideInput();

        // switch (_inputValue.InputType)
        // {
        //     case InputValueTypes.String:
        //         _stringInput.gameObject.SetActive(true);
        //         _stringInput.SetValue(_inputValue.Value != null ? _inputValue.Value.ToString() : "");
        //         break;
        //     case InputValueTypes.Number:
        //         _numberInput.gameObject.SetActive(true);
        //         _numberInput.SetValue(_inputValue.Value != null ? (float)_inputValue.Value : 0);
        //         break;
        //     case InputValueTypes.Boolean:
        //         _booleanInput.gameObject.SetActive(true);
        //         _booleanInput.SetValue(_inputValue.Value != null && (bool)_inputValue.Value);
        //         break;
        //     case InputValueTypes.Entity:
        //         _dropdown.gameObject.SetActive(true);
        //         _dropdown.AddOptions(ObjectManager.Instance.GetEntityOptions());
        //         _height = 60f;
        //         break;
        //     case InputValueTypes.Variable:
        //         _dropdown.gameObject.SetActive(true);
        //         _dropdown.AddOptions(NodeBoard.Instance.TargetVisualScripting.GetVariableOptions());
        //         _height = 60f;
        //         break;
        //     default:
        //         _stringInput.gameObject.SetActive(false);
        //         _dropdown.gameObject.SetActive(false);
        //         _height = 30f;
        //         break;
        // }

        UpdateSize();
    }

    private void UpdateSize()
    {
        Vector2 size = _label.GetPreferredValues();
        _label.rectTransform.sizeDelta = new Vector2
        (
            size.x,
            _label.rectTransform.sizeDelta.y
        );

        float width = _handleSize + size.x + _inputOffset;

        switch (_inputValue.InputType)
        {
            case InputValueTypes.String:
                _stringInput.Rect.anchoredPosition = new Vector2(width + _inputOffset, 0f);
                width += _stringInput.Rect.sizeDelta.x;
                break;
            case InputValueTypes.Number:
                _numberInput.Rect.anchoredPosition = new Vector2(width + _inputOffset, 0f);
                width += _stringInput.Rect.sizeDelta.x;
                break;
            case InputValueTypes.Boolean:
                _booleanInput.Rect.anchoredPosition = new Vector2(width + _inputOffset, 0f);
                width += _booleanInput.Rect.sizeDelta.x;
                break;
            case InputValueTypes.Entity:
                width = 150f;
                break;
            case InputValueTypes.Variable:
                width = 150f;
                break;
            default:
                width = _handleSize + size.x;
                break;
        }

        Rect.sizeDelta = new Vector2(width, _height);
    }

    private void HideInput()
    {
        _stringInput.gameObject.SetActive(false);
        _numberInput.gameObject.SetActive(false);
        _booleanInput.gameObject.SetActive(false);
        _dropdown.gameObject.SetActive(false);
    }

    private void ShowInput()
    {
        switch (_inputValue.InputType)
        {
            case InputValueTypes.String:
                _stringInput.gameObject.SetActive(true);
                break;
            case InputValueTypes.Number:
                _numberInput.gameObject.SetActive(true);
                break;
            case InputValueTypes.Boolean:
                _booleanInput.gameObject.SetActive(true);
                break;
            case InputValueTypes.Entity:
            case InputValueTypes.Variable:
                _dropdown.gameObject.SetActive(true);
                break;
        }
    }

    public override void ValidConnection(IPort port)
    {
        for (int i = 0; i < _lineConnections.Count; i++)
        {
            if (_lineConnections[i].Source.Port != port)
            {
                _lineConnections[i].Delete();
            }
        }
    }

    public override void AfterAdd()
    {
        if (_inputValue.HasConnection)
        {
            HideInput();
        }
        else
        {
            ShowInput();
        }
    }


    #region Handle Input Event

    private void OnStringInputSubmit(string value)
    {
        _inputValue.SetValue(value);
    }

    private void OnStringInputValueUpdated()
    {
        Rect.sizeDelta = new Vector2
        (
            _handleSize + _label.rectTransform.sizeDelta.x + _inputOffset + _stringInput.Rect.sizeDelta.x,
            _height
        );

        UINode.UpdateSize();
    }

    private void OnNumberInputSubmit(float value)
    {
        _inputValue.SetValue(value);
    }

    private void OnNumberInputValueUpdated()
    {
        Rect.sizeDelta = new Vector2
        (
            _handleSize + _label.rectTransform.sizeDelta.x + _inputOffset + _numberInput.Rect.sizeDelta.x,
            _height
        );

        UINode.UpdateSize();
    }

    private void OnBooleanInputSubmit(bool value)
    {
        _inputValue.SetValue(value);
    }

    private void OnDropDownValueChanged(int index)
    {
        if (Port is InputValue valueInput)
        {
            valueInput.SetValue(index);
        }
    }

    #endregion
}