using System;
using TMPro;
using UnityEngine;

public class UIValueInputPort : UINodePort
{
    public override NodePortEdge Edge => NodePortEdge.Left;
    private InputValue _inputValue;

    [SerializeField] private PortVisual _portVisual;
    
    [SerializeField] private RectTransform _inputHolder;
    [SerializeField] private UIInputData _inputDataReference;
    private BaseInput _input;

    private float _height = 30f;
    private readonly float _handleSize = 60f;
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

        _portVisual.SetType(_inputValue.Type);

        if (_inputValue.InputType == InputValueTypes.None)
        {
            UpdateSize();
            return;
        }

        _input = _inputDataReference.Get(_inputValue.InputType, UINode.Board.Entity);
        _input.Rect.SetParent(_inputHolder, false);
        _input.SetValue(_inputValue.Value);

        _inputValue.SetValue(_input.GetValue());

        _input.OnSubmit += OnSubmit;

        UpdateSize();
    }

    private void UpdateSize()
    {
        Vector2 labelSize = _label.GetPreferredValues();
        _label.rectTransform.sizeDelta = new Vector2
        (
            labelSize.x,
            _label.rectTransform.sizeDelta.y
        );

        float width = _handleSize + labelSize.x + _inputOffset;

        _inputHolder.anchoredPosition = new Vector2(width, 0f);

        if (_input != null)
        {
            width += _input.Size.x;
        }

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
        for (int i = 0; i < LineConnections.Count; i++)
        {
            if (LineConnections[i].Source.Port != port)
            {
                LineConnections[i].Delete();
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

    private void OnSubmit(object value)
    {
        Debug.Log("submit " + value);
        Debug.Log($"Set value: {value}");
        _inputValue.SetValue(value);
    }
}