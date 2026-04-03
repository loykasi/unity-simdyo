using System;
using Loykas.Scripting;
using UnityEngine;

public class TracerMenu : MonoBehaviour
{
    public bool IsOpened;

    [Header("Entity")]
    [SerializeField] private StringInput _idInput;
    [SerializeField] private StringInput _nameInput;

    [Header("Tracer Properties")]
    [SerializeField] private NumberInput _timeInput;
    [SerializeField] private NumberInput _diameterInput;
    [SerializeField] private ColorInput _colorInput;

    private TracerEntity _entity;

    private void Awake()
    {
        _nameInput.OnSubmit += OnNameSubmit;
        _timeInput.OnSubmit += OnTimeSubmit;
        _diameterInput.OnSubmit += OnDiameterSubmit;
        _colorInput.OnSubmit += OnColorSubmit;
    }

    public void Open(TracerEntity entity)
    {
        gameObject.SetActive(true);
        _entity = entity;
        _entity.OnPropertyUpdated += OnPropertyUpdated;

        Init();
        IsOpened = true;
    }

    public void Close()
    {
        if (IsOpened)
        {
            gameObject.SetActive(false);
            _entity.OnPropertyUpdated -= OnPropertyUpdated;
            _entity = null;
            IsOpened = false;
        }
    }

    private void OnPropertyUpdated()
    {
        Init();
    }

    private void Init()
    {
        _idInput.SetValue(_entity.Id.ToString());
        _nameInput.SetValue(_entity.Name);
        _timeInput.SetValue(_entity.Time);
        _diameterInput.SetValue(_entity.Diameter);
    }

    public void OnNameSubmit(object value)
    {
        ObjectManager.Instance.RenameEntity(_entity, (string)value);
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
}