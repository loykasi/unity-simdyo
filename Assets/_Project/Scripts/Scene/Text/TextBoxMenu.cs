using System;
using Loykas.Scripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TextBoxMenu : Singleton<TextBoxMenu>, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _window;
    [SerializeField] private StringInput _input;
    [SerializeField] private ColorInput _colorInput;
    [SerializeField] private NumberInput _sizeInput;

    [Header("Alignment")]
    [SerializeField] private Button _alignLeft;
    [SerializeField] private Button _alignCenter;
    [SerializeField] private Button _alignRight;
    [SerializeField] private Button _verticalAlignTop;
    [SerializeField] private Button _verticalAlignMiddle;
    [SerializeField] private Button _verticalAlignBottom;
    
    private TextBox _targetTextBox;
    private bool _isMouseOver = false;

    private void Start()
    {
        _input.OnValueUpdated += OnValueChanged;
        _colorInput.OnSubmit += OnColorChanged;
        _sizeInput.OnSubmit += OnSizeChanged;

        _alignLeft.onClick.AddListener(AlignLeft);
        _alignCenter.onClick.AddListener(AlignCenter);
        _alignRight.onClick.AddListener(AlignRight);
        _verticalAlignTop.onClick.AddListener(AlignVerticalTop);
        _verticalAlignMiddle.onClick.AddListener(AlignVerticalMiddle);
        _verticalAlignBottom.onClick.AddListener(AlignVerticalBottom);
    }

    // private void Update()
    // {
    //     if (Mouse.current.leftButton.wasPressedThisFrame && !_isMouseOver)
    //     {
    //         _window.SetActive(false);
    //     }
    // }

    private void OnValueChanged()
    {
        _targetTextBox.Text = (string)_input.GetValue();
    }

    private void OnColorChanged(object value)
    {
        ColorHSV color = (ColorHSV)value;
        _targetTextBox.Color = color.ToUnityColor();
    }

    private void OnSizeChanged(object value)
    {
        _targetTextBox.Size = (float)value;
    }

    private void AlignLeft()
    {
        _targetTextBox.HorizontalAlignment = TMPro.HorizontalAlignmentOptions.Left;
    }

    private void AlignCenter()
    {
        _targetTextBox.HorizontalAlignment = TMPro.HorizontalAlignmentOptions.Center;
    }

    private void AlignRight()
    {
        _targetTextBox.HorizontalAlignment = TMPro.HorizontalAlignmentOptions.Right;
    }

    private void AlignVerticalTop()
    {
        _targetTextBox.VerticalAlignment = TMPro.VerticalAlignmentOptions.Top;
    }

    private void AlignVerticalMiddle()
    {
        _targetTextBox.VerticalAlignment = TMPro.VerticalAlignmentOptions.Middle;
    }

    private void AlignVerticalBottom()
    {
        _targetTextBox.VerticalAlignment = TMPro.VerticalAlignmentOptions.Bottom;
    }

    public void Open(TextBox textBox)
    {
        _targetTextBox = textBox;

        _input.SetValue(_targetTextBox.Text);
        _colorInput.SetValue(new ColorHSV(_targetTextBox.Color));
        _sizeInput.SetValue(_targetTextBox.Size);

        _window.SetActive(true);
    }

    public void Close()
    {
        _targetTextBox = null;
        _window.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isMouseOver = false;
    }
}