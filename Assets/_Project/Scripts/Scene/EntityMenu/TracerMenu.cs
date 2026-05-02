using System;
using Loykas.Scripting;
using UnityEngine;
using UnityEngine.UI;

public class TracerMenu : BaseEntityMenu
{
    [Header("Properties")]
    [SerializeField] private NumberInput _timeInput;
    [SerializeField] private NumberInput _diameterInput;
    [SerializeField] private ColorInput _colorInput;
    [SerializeField] private Toggle _hideIndicatorOnStartToggle;

    private TracerEntity _entity;

    private void OnPropertyUpdated()
    {
        UpdateUI();
    }

    private void OnTimeSubmit(object value)
    {
        _entity.Time = (float)value;
    }

    private void OnDiameterSubmit(object value)
    {
        _entity.Diameter = (float)value;
    }

    private void OnColorSubmit(object value)
    {
        _entity.Color = (ColorHSV)value;
    }

    private void ToggleHideIndicatorOnStart(bool value)
    {
        _entity.ShouldHideIndicatorOnStart = value;
    }

    public override void Setup()
    {
        _timeInput.OnSubmit += OnTimeSubmit;
        _diameterInput.OnSubmit += OnDiameterSubmit;
        _colorInput.OnSubmit += OnColorSubmit;
        _hideIndicatorOnStartToggle.onValueChanged.AddListener(ToggleHideIndicatorOnStart);
    }

    public override void UpdateUI()
    {
        if (_entity == null)
        {
            return;
        }

        _timeInput.SetValue(_entity.Time);
        _diameterInput.SetValue(_entity.Diameter);
        _colorInput.SetValue(_entity.Color);
    }

    public override void AddEntity(SceneEntity entity)
    {
        if (entity is TracerEntity tracer)
        {
            _entity = tracer;
            _entity.OnPropertyUpdated += OnPropertyUpdated;
        }
    }

    public override void Clear()
    {
        if (_entity != null)
        {
            _entity.OnPropertyUpdated -= OnPropertyUpdated;
            _entity = null;
        }
    }

    public override void Show()
    {
        if (_entity != null)
        {
            _menuGameObject.SetActive(true);
        }
    }

    public override void Close()
    {
        Clear();
        _menuGameObject.SetActive(false);
    }
}