using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ObjectManager : Singleton<ObjectManager>, ISaveable
{
    public event UnityAction<SceneEntity> OnObjectSelected;
    public event UnityAction OnObjectDeselected;
    public event UnityAction OnObjectDeleted;

    public List<SceneEntity> SceneEntities = new();
    public SceneEntity SelectedObject { get; set; }
    public int SaveLoadOrder { get; set; } = 0;

    // [SerializeField] private float _selectRadius;
    // [SerializeField] private int _defaultLayer;
    // [SerializeField] private int _selectLayer;

    // temporary
    private int _indexForID = 0;

    public void Select(Vector3 screenPoint)
    {
        Camera camera = EngineManager.Instance.EditorCamera;
        Ray ray = camera.ScreenPointToRay(screenPoint);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        Vector3 worldPoint = camera.ScreenToWorldPoint(screenPoint);

        if (hit.collider == null)
        {
            if (SelectedObject != null)
            {
                SelectedObject.Deselect();
                SelectedObject = null;
            }

            OnObjectDeselected?.Invoke();
            return;
        }

        if (SelectedObject != null)
        {
            SelectedObject.Deselect();
        }

        SelectedObject = hit.collider.GetComponent<SceneEntity>();
        SelectedObject.Select();

        OnObjectSelected?.Invoke(SelectedObject);
    }

    public List<string> GetEntityOptions()
    {
        var list = SceneEntities.Select(e => e.ID).ToList();
        list.Insert(0, "Null");
        return list;
    }

    public SceneEntity GetEntityByIndex(int index)
    {
        index--;

        if (index == -1)
        {
            return null;
        }
        
        return SceneEntities[index];
    }

    public void AddEntity(SceneEntity entity)
    {
        SceneEntities.Add(entity);

        // Temporary Method for Set ID
        // use it for both ID and Name now, will sperate in futures
        entity.ID = string.Concat("Entity" + (_indexForID == 0 ? "" : $" {_indexForID}"));
        _indexForID++;
    }

    public SceneEntity GetEntity(string id)
    {
        return SceneEntities.Find(entity => entity.ID == id);
    }

    public void DeleteEntity(SceneEntity entity)
    {
        if (SelectedObject == entity)
        {
            SelectedObject = null;
            OnObjectDeselected?.Invoke();
        }

        SceneEntities.Remove(entity);
        Destroy(entity.gameObject);
    }

    public void SaveData(GameData data)
    {
        data.Scene.Entities.Clear();
        foreach (var entity in SceneEntities)
        {
            EntityData entityData = entity.EntityType switch
            {
                EntityType.Box => new BoxEntityData
                {
                    Width = ((BoxEntity)entity).Width,
                    Height = ((BoxEntity)entity).Height,
                },
                EntityType.Circle => new CircleEntityData
                {
                    Radius = ((CircleEntity)entity).Radius
                },
                _ => new(),
            };

            entityData.Type = entity.EntityType;
            entityData.Position = entity.transform.position;
            entityData.Rotation = entity.transform.rotation;
            entityData.Color = entity.CurrentColor;
            entityData.TextureSlot = entity.TextureSlot;

            entityData.Script.Nodes = entity.Script.Nodes;
            entityData.Script.Connections = entity.Script.Connections;
            entityData.Script.Variables = entity.Script.Variables;

            data.Scene.Entities.Add(entityData);
        }
    }

    public void LoadData(GameData data)
    {
        foreach (var entity in SceneEntities)
        {
            Destroy(entity.gameObject);
        }
        SceneEntities.Clear();

        Debug.Log(data.Scene.Entities.Count);

        foreach (var entity in data.Scene.Entities)
        {
            switch (entity.Type)
            {
                case EntityType.Box:
                    BoxEntityData boxData = (BoxEntityData)entity;
                    BoxEntity box = ShapeGenerator.Instance.AddBox(entity.Position, boxData.Width, boxData.Height);
                    box.CurrentColor = boxData.Color;
                    if (boxData.TextureSlot != -1)
                    {
                        box.SetTexture(boxData.TextureSlot, AssetController.Instance.Textures[boxData.TextureSlot]);
                    }
                    box.Script.Nodes.AddRange(entity.Script.Nodes);
                    box.Script.Connections.AddRange(entity.Script.Connections);
                    box.Script.Variables = entity.Script.Variables;
                    break;
                case EntityType.Circle:
                    CircleEntityData circleData = (CircleEntityData)entity;
                    CircleEntity circle = ShapeGenerator.Instance.AddCircle(circleData.Position, circleData.Radius);
                    circle.CurrentColor = circleData.Color;
                    if (circle.TextureSlot != -1)
                    {
                        circle.SetTexture(circleData.TextureSlot, AssetController.Instance.Textures[circleData.TextureSlot]);
                    }
                    circle.Script.Nodes = entity.Script.Nodes;
                    circle.Script.Connections = entity.Script.Connections;
                    circle.Script.Variables = entity.Script.Variables;
                    break;
            }
        }

        foreach (var entity in SceneEntities)
        {
            entity.Script.Load();
        }
    }
}