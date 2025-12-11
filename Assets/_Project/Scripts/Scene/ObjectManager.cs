using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Loykas.Scripting;

public class ObjectManager : Singleton<ObjectManager>, ISaveable
{
    public event UnityAction<SceneEntity> OnObjectSelected;
    public event UnityAction OnObjectDeselected;
    public event UnityAction OnObjectDeleted;

    public List<SceneEntity> SceneEntities = new();
    public SceneEntity SelectedObject { get; set; }
    public int SaveLoadOrder { get; set; } = 0;

    [SerializeField] private Transform _holder;

    // use on running scene
    private List<SceneEntity> _snapshotEntities = new();

    // temporary
    private int _indexForID = 0;

    private void OnEnable()
    {
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.OnSceneStart += OnSceneStart;
            SceneManager.Instance.OnSceneStop += OnSceneStop;
        }
    }

    private void OnDisable()
    {
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.OnSceneStart -= OnSceneStart;
            SceneManager.Instance.OnSceneStop -= OnSceneStop;
        }
    }

    public void Deselect()
    {
        if (SelectedObject != null)
        {
            SelectedObject.Deselect();
            SelectedObject = null;

            OnObjectDeselected?.Invoke();
        }
    }

    public void Select(Vector3 screenPoint)
    {
        Deselect();

        if (!TryGetSceneEntity(EngineManager.Instance.EditorCamera, screenPoint, out SceneEntity entity))
        {
            return;
        }

        SelectedObject = entity;
        SelectedObject.Select();

        OnObjectSelected?.Invoke(SelectedObject);
    }

    public void Click(Vector3 screenPoint)
    {
        if (!TryGetSceneEntity(EngineManager.Instance.SceneCamera, screenPoint, out SceneEntity entity))
        {
            return;
        }

        entity.Script.TriggerEvent(EventHook.Clicked);
    }

    private bool TryGetSceneEntity(Camera camera, Vector3 screenPoint, out SceneEntity entity)
    {
        Ray ray = camera.ScreenPointToRay(screenPoint);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        Debug.DrawRay(ray.origin, Vector3.up * 3f, Color.red, 10f);

        if (hit.collider == null)
        {
            entity = null;
            return false;
        }

        entity = hit.collider.GetComponent<SceneEntity>();
        return true;
    }

    public List<string> GetEntityOptions()
    {
        var list = SceneEntities.Select(e => e.ID).ToList();
        list.Insert(0, "Null");
        return list;
    }

    public int GetIndexByEntity(SceneEntity entity)
    {
        return SceneEntities.FindIndex(e => e == entity) + 1;
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
    
    public void AddBox(Vector3 from, Vector3 to)
    {
        SceneEntity entity = ShapeGenerator.Instance.AddBox(from, to);
        AddEntity(entity);
    }

    public void AddCircle(Vector3 from, Vector3 to)
    {
        SceneEntity entity = ShapeGenerator.Instance.AddCircle(from, to);
        AddEntity(entity);
    }

    public void AddEntity(SceneEntity entity)
    {
        // Temporary Method for Set ID
        // use it for both ID and Name now, will sperate in futures
        entity.ID = string.Concat("Entity" + (_indexForID == 0 ? "" : $" {_indexForID}"));
        _indexForID++;

        entity.transform.SetParent(_holder);
        SceneEntities.Add(entity);

        if (SceneManager.Instance.IsRuning)
        {
            entity.IsDirty = true;
        }
    }

    public void DeleteEntity(SceneEntity entity)
    {
        if (SelectedObject == entity)
        {
            SelectedObject = null;
            OnObjectDeselected?.Invoke();
        }

        SceneEntities.Remove(entity);

        if (SceneManager.Instance.IsRuning)
        {
            if (entity.IsDirty)
            {
                Destroy(entity.gameObject);
            }
            else
            {
                entity.gameObject.SetActive(false);
            }
        }
        else
        {
            Destroy(entity.gameObject);
        }
    }

    public SceneEntity GetEntity(string id)
    {
        return SceneEntities.Find(entity => entity.ID == id);
    }

    private void OnSceneStart()
    {
        _snapshotEntities.AddRange(SceneEntities);
    }

    private void OnSceneStop()
    {
        for (int i = SceneEntities.Count - 1; i >= 0; i--)
        {
            SceneEntity entity = SceneEntities[i];
            if (entity.IsDirty)
            {
                Destroy(entity.gameObject);
                SceneEntities.RemoveAt(i);
            }
        }

        SceneEntities.Clear();
        SceneEntities.AddRange(_snapshotEntities);
        
        for (int i = 0; i < SceneEntities.Count; i++)
        {
            SceneEntity entity = SceneEntities[i];
            entity.gameObject.SetActive(true);
        }
        _snapshotEntities.Clear();
    }

    public void ResetState()
    {
        if (SelectedObject != null)
        {
            SelectedObject.Deselect();
            SelectedObject = null;
        }

        for (int i = 0; i < SceneEntities.Count; i++)
        {
            Destroy(SceneEntities[i].gameObject);
        }

        SceneEntities.Clear();
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
            entityData.GravityEnabled = entity.IsGravityEnabled;
            entityData.ColliderEnabled = entity.IsColliderEnabled;
            entityData.Layer = entity.Layer;
            entityData.Color = entity.CurrentColor;
            entityData.TextureSlot = entity.TextureSlot;

            ScriptSaveHandler.Save(entityData.Script, entity.Script);

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

        Debug.Log($"Load {data.Scene.Entities.Count} objects");

        foreach (var entityData in data.Scene.Entities)
        {
            SceneEntity entity;
            switch (entityData.Type)
            {
                case EntityType.Box:
                    BoxEntityData boxData = (BoxEntityData)entityData;
                    entity = ShapeGenerator.Instance.AddBox(boxData.Position, boxData.Width, boxData.Height);
                    break;
                case EntityType.Circle:
                    CircleEntityData circleData = (CircleEntityData)entityData;
                    entity = ShapeGenerator.Instance.AddCircle(circleData.Position, circleData.Radius);
                    break;
                default:
                    continue;
            }

            entity.Rotation = entityData.Rotation;
            entity.CurrentColor = entityData.Color;

            entity.SetCollider(entityData.ColliderEnabled);
            entity.SetGravity(entityData.GravityEnabled);
            entity.SetLayer(entityData.Layer);
            
            if (entityData.TextureSlot > 0)
            {
                entity.SetTexture(entityData.TextureSlot);
            }
            
            ScriptSaveHandler.Load(entityData.Script, entity.Script);
        }
    }
}