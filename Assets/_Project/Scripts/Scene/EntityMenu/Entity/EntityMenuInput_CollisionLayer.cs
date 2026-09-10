using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityMenuInput_CollisionLayer : EntityMenuInputBase
{
    [SerializeField] private RectTransform _collisionLayer;
    [SerializeField] private CollisionLayerToggle _layerTogglePrefab;
    [SerializeField] private Transform _layerHolder;
    
    private CollisionLayerToggle[] _collisionLayerToggles;
    private CollisionLayer _layer;
    
    public void Setup()
    {
        var layers = Enum.GetValues(typeof(CollisionLayer));
        _collisionLayerToggles = new CollisionLayerToggle[layers.Length];

        int i = 0;
        foreach (CollisionLayer layer in layers)
        {
            CollisionLayerToggle layerToggle = Instantiate(_layerTogglePrefab, _layerHolder);

            layerToggle.Init(layer, ToggleLayer);
            _collisionLayerToggles[i] = layerToggle;
            i++;
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
        for (int i = 0; i < _collisionLayerToggles.Length; i++)
        {
            var toggle = _collisionLayerToggles[i];
            UpdateToggle(toggle);
        }
    }

    private void UpdateToggle(CollisionLayerToggle toggle)
    {
        List<SceneEntity> entities = ObjectManager.Instance.SelectionGroup.Entities;
        int count = entities.Count;
        EntityProperty property = null;

        for (int j = 0; j < count; j++)
        {
            EntityProperty check = entities[j].GetProperty(_type);
            if (check == null) continue;

            if (property == null)
            {
                property = check;
                continue;
            }

            CollisionLayer a = (CollisionLayer)property.Get();
            CollisionLayer b = (CollisionLayer)check.Get();
            
            if (((a & toggle.Layer) != 0) != ((b & toggle.Layer) != 0))
            {
                toggle.SetUnknownState();
                return;
            }
        }

        CollisionLayer layer = (CollisionLayer)property.Get();
        toggle.SetState((layer & toggle.Layer) != 0);
    }

    private void OnChanged()
    {
        UpdateInternal();
    }

    public void ToggleLayer(CollisionLayer layer, bool isActive)
    {
        List<SceneEntity> entities = ObjectManager.Instance.SelectionGroup.Entities;
        int count = entities.Count;
        
        for (int i = 0; i < count; i++)
        {
            if (entities[i] is MeshEntity meshEntity)
            {
                meshEntity.SetLayer(layer, isActive);
            }
        }
    }
}