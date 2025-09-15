using System;
using TMPro;
using UnityEngine;

public class UIValueInputPort : UINodePort
{
    public override NodePortEdge Edge => NodePortEdge.Left;
    private InputValue _inputValue;

    [SerializeField] private RectTransform _inputHolder;
    [SerializeField] private UIInputData _inputDataReference;
    private BaseInput _input;

    private float _height = 60f;
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

        if (_inputValue.InputType == InputValueTypes.None)
        {
            return;
        }

        _input = _inputDataReference.Get(_inputValue.InputType, UINode.Board.Entity);
        _input.Rect.SetParent(_inputHolder, false);
        _input.OnSubmit += OnSubmit;

        // switch (_inputValue.InputType)
        // {
        //     case InputValueTypes.String:
        //         _input.Rect.SetParent(_inputHolder1, false);
        //         break;
        //     case InputValueTypes.Number:
        //         _input.Rect.SetParent(_inputHolder1, false);
        //         break;
        //     case InputValueTypes.Boolean:
        //         _input.Rect.SetParent(_inputHolder1, false);
        //         break;
        //     case InputValueTypes.Entity:
        //         _input.Rect.SetParent(_inputHolder1, false);
        //         _height = 60f;
        //         break;
        //     case InputValueTypes.Variable:
        //         _input.Rect.SetParent(_inputHolder1, false);
        //         _height = 60f;
        //         break;
        //     default:
        //         _input.Rect.SetParent(_inputHolder1, false);
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

        _inputHolder.anchoredPosition = new Vector2(width, 0f);

        width += _input.Size.x;

        Rect.sizeDelta = new Vector2(width, _height);
    }

    private void HideInput()
    {
        _inputHolder.gameObject.SetActive(false);
    }

    private void ShowInput()
    {
        _inputHolder.gameObject.SetActive(true);
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

    private void OnSubmit(object value)
    {
        Debug.Log($"Set value: {value}");
        _inputValue.SetValue(value);
    }

    // private void OnStringInputSubmit(string value)
    // {
    //     _inputValue.SetValue(value);
    // }

    // private void OnStringInputValueUpdated()
    // {
    //     Rect.sizeDelta = new Vector2
    //     (
    //         _handleSize + _label.rectTransform.sizeDelta.x + _inputOffset + _stringInput.Rect.sizeDelta.x,
    //         _height
    //     );

    //     UINode.UpdateSize();
    // }

    // private void OnNumberInputSubmit(float value)
    // {
    //     _inputValue.SetValue(value);
    // }

    // private void OnNumberInputValueUpdated()
    // {
    //     Rect.sizeDelta = new Vector2
    //     (
    //         _handleSize + _label.rectTransform.sizeDelta.x + _inputOffset + _numberInput.Rect.sizeDelta.x,
    //         _height
    //     );

    //     UINode.UpdateSize();
    // }

    // private void OnBooleanInputSubmit(bool value)
    // {
    //     _inputValue.SetValue(value);
    // }

    // private void OnDropDownValueChanged(int index)
    // {
    //     if (Port is InputValue valueInput)
    //     {
    //         valueInput.SetValue(index);
    //     }
    // }

    #endregion
}