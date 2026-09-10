using System.Collections.Generic;
using UnityEngine;

public class EntityMenuInput_Vector : EntityMenuInputBase
{
    [SerializeField] private NumberInput _xInput;
    [SerializeField] private NumberInput _yInput;

    private Vector3 _value;
    private bool _isVector2;

    private void Start()
    {
        _xInput.OnSubmit += OnXSubmit;
        _yInput.OnSubmit += OnYSubmit;
    }

    private void OnDestroy()
    {
        _xInput.OnSubmit -= OnXSubmit;
        _yInput.OnSubmit -= OnYSubmit;
    }

    void OnDisable()
    {
        if (ObjectManager.Instance != null)
        {
            List<SceneEntity> entities = ObjectManager.Instance.SelectionGroup.Entities;
            int count = entities.Count;

            for (int i = 0; i < count; i++)
            {
                var property = entities[i].GetProperty(_type);
                property.ChangedEvent -= OnChanged;
            }   
        }
    }

    public override void UpdateValue()
    {
        if (_type == EntityPropertyType.None)
        {
            return;
        }

        UpdateInternal();
        
        List<SceneEntity> entities = ObjectManager.Instance.SelectionGroup.Entities;
        int count = entities.Count;

        for (int i = 0; i < count; i++)
        {
            var property = entities[i].GetProperty(_type);
            if (property == null) continue;
            property.ChangedEvent -= OnChanged;
            property.ChangedEvent += OnChanged;
        }
    }

    private void UpdateInternal()
    {
        List<SceneEntity> entities = ObjectManager.Instance.SelectionGroup.Entities;
        int count = entities.Count;

        EntityProperty property = null;
        for (int i = 0; i < count; i++)
        {
            EntityProperty check = entities[i].GetProperty(_type);
            if (check == null) continue;

            if (property == null)
            {
                property = check;
                var value = property.Get();
                // walkaround for now and I *definitely* need to update this after i done with property menu features
                switch (value)
                {
                    case Vector2 vector2Value:
                        _isVector2 = true;
                        _value = vector2Value;
                        break;
                    case Vector3 vector3Value:
                        _isVector2 = false;
                        _value = vector3Value;
                        break;
                    default:
                        _isVector2 = false;
                        _value = default;
                        break;
                }
                continue;
            }

            if (!property.IsEqual(entities[i].GetProperty(_type)))
            {
                _xInput.SetUnknownState();
                _yInput.SetUnknownState();
                return;
            }
        }
        
        _xInput.SetValue(_value.x);
        _yInput.SetValue(_value.y);
    }

    private void OnChanged()
    {
        UpdateInternal();
    }

    private void OnXSubmit(object value)
    {
        _value.x = (float)value;
        List<SceneEntity> entities = ObjectManager.Instance.SelectionGroup.Entities;
        int count = entities.Count;

        for (int i = 0; i < count; i++)
        {
            entities[i].GetProperty(_type)?.Set(_isVector2 ? (Vector2)_value : _value);
        }
    }

    private void OnYSubmit(object value)
    {
        _value.y = (float)value;
        List<SceneEntity> entities = ObjectManager.Instance.SelectionGroup.Entities;
        int count = entities.Count;

        for (int i = 0; i < count; i++)
        {
            entities[i].GetProperty(_type)?.Set(_isVector2 ? (Vector2)_value : _value);
        }
    }
}