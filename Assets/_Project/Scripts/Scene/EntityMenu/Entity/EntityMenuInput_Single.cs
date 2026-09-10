using System.Collections.Generic;
using Loykas.Scripting;
using UnityEngine;

public class EntityMenuInput_Single<T> : EntityMenuInputBase where T : BaseInput
{
    [SerializeField] private T _input;

    private void Start()
    {
        _input.OnSubmit += OnSubmit;
    }

    private void OnDestroy()
    {
        _input.OnSubmit -= OnSubmit;
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
                continue;
            }
            
            if (!property.IsEqual(check))
            {
                _input.SetUnknownState();
                return;
            }
        }

        if (property != null)
        {
            gameObject.SetActive(true);
            _input.SetValue(property.Get());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnChanged()
    {
        UpdateInternal();
    }

    private void OnSubmit(object value)
    {
        List<SceneEntity> entities = ObjectManager.Instance.SelectionGroup.Entities;
        int count = entities.Count;

        for (int i = 0; i < count; i++)
        {
            if (_type is EntityPropertyType.ZDepth)
            {
                entities[i].GetProperty(_type)?.Set((int)(float)value);
            }
            else
            {
                entities[i].GetProperty(_type)?.Set(value);
            }
        }
    }
}