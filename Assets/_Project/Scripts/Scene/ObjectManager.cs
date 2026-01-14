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
    [SerializeField] private LayerMask _interactionLayer;

    private int _indexForId = 1;

    private List<SceneEntity> _snapshotEntities = new();    // use on running scene
    private List<string> _entityNames = new();    // For generate unique name
    private readonly string _baseEntityName = "Entity";

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
        SceneManager.Instance.GlobalScript.TriggerEvent(EventHook.Clicked);

        if (!TryGetSceneEntity(EngineManager.Instance.SceneCamera, screenPoint, out SceneEntity entity))
        {
            return;
        }
        
        entity.Script.TriggerEvent(EventHook.Clicked);
    }

    private bool TryGetSceneEntity(Camera camera, Vector3 screenPoint, out SceneEntity entity)
    {
        Ray ray = camera.ScreenPointToRay(screenPoint);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 20f, _interactionLayer);

        Debug.DrawRay(ray.origin, Vector3.up * 3f, Color.red, 10f);

        if (hit.collider == null)
        {
            entity = null;
            return false;
        }

        entity = hit.collider.GetComponent<Interactable>().Get();
        return true;
    }

    public SceneEntity GetEntityById(int id)
    {
        return SceneEntities.Find(e => e.Id == id);
    }

    public List<string> GetEntityOptions(SceneEntity entity = null)
    {
        var list = SceneEntities.Select(e => e.Name).ToList();
        list.Insert(0, entity == null ? "Null" : "This");
        return list;
    }

    public List<string> GetSignalEntityOptions()
    {
        var list = SceneEntities.Select(e => e.Name).ToList();
        list.Insert(0, "All");
        return list;
    }

    // public int GetIndexByEntity(SceneEntity entity)
    // {
    //     return SceneEntities.FindIndex(e => e == entity) + 1;
    // }

    public int GetIndexByEntityID(int id)
    {
        return SceneEntities.FindIndex(e => e.Id == id) + 1;
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
        if (entity == null)
        {
            return;
        }

        AddEntity(entity);
    }

    public SceneEntity AddBox(Vector3 center, float width, float height)
    {
        SceneEntity entity = ShapeGenerator.Instance.AddBox(center, width, height);
        if (entity == null)
        {
            return null;
        }

        AddEntity(entity);
        return entity;
    }

    public void AddCircle(Vector3 from, Vector3 to)
    {
        SceneEntity entity = ShapeGenerator.Instance.AddCircle(from, to);
        if (entity == null)
        {
            return;
        }

        AddEntity(entity);
    }

    public SceneEntity AddCircle(Vector3 center, float radius)
    {
        SceneEntity entity = ShapeGenerator.Instance.AddCircle(center, radius);
        if (entity == null)
        {
            return null;
        }

        AddEntity(entity);
        return entity;
    }

    public void AddEntity(SceneEntity entity)
    {
        // use for both ID and Name now, will sperate in futures
        // entity.Id = string.Concat("Entity" + (_indexForID == 0 ? "" : $" {_indexForID}"));
        entity.Id = _indexForId++;

        // name
        string entityName = GetEntityName(_baseEntityName);
        entity.Name = entityName;

        // z depth
        int depth = -1;
        for (int i = 0; i < SceneEntities.Count; i++)
        {
            SceneEntity target = SceneEntities[i];
            if (target.ZDepth > depth)
            {
                depth = target.ZDepth;
            }
        }
        entity.ZDepth = depth + 1;

        if (SceneManager.Instance.IsRuning)
        {
            entity.IsDirty = true;
        }

        entity.transform.SetParent(_holder);
        SceneEntities.Add(entity);
    }

    private string GetEntityName(string baseName)
    {
        _entityNames.Clear();
        for (int i = 0; i < SceneEntities.Count; i++)
        {
            _entityNames.Add(SceneEntities[i].Name);
        }

        return Utils.GenerateUniqueName(baseName, _entityNames);
    }

    public void RenameEntity(SceneEntity entity, string name)
    {
        int index = SceneEntities.FindIndex(e => e.Name == name);
        entity.Name = index == -1 ? name : GetEntityName(_baseEntityName);
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

    public void MoveToBack(SceneEntity entity)
    {
        for (int i = 0; i < SceneEntities.Count; i++)
        {
            SceneEntity target = SceneEntities[i];
            if (target.ZDepth < entity.ZDepth)
            {
                target.ZDepth++;
            }
        }
        entity.ZDepth = 0;
    }

    public void MoveToFront(SceneEntity entity)
    {
        int depth = 0;
        for (int i = 0; i < SceneEntities.Count; i++)
        {
            SceneEntity target = SceneEntities[i];
            if (target.ZDepth > depth)
            {
                depth = target.ZDepth;
            }
            if (target.ZDepth > entity.ZDepth)
            {
                target.ZDepth--;
            }
        }
        entity.ZDepth = depth;
    }

    public SceneEntity GetEntity(int id)
    {
        return SceneEntities.Find(entity => entity.Id == id);
    }

    private void OnSceneStart()
    {
        _snapshotEntities.Clear();
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
        _snapshotEntities.AddRange(SceneEntities);
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
        _indexForId = 1;
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
                    Text = ((BoxEntity)entity).TextBox.Text,
                    TextColor = new ColorHSV(((BoxEntity)entity).TextBox.Color),
                    TextSize = ((BoxEntity)entity).TextBox.Size,
                    TextHorizontalAlignment = ((BoxEntity)entity).TextBox.HorizontalAlignment,
                    TextVerticalAlignment = ((BoxEntity)entity).TextBox.VerticalAlignment,
                },
                EntityType.Circle => new CircleEntityData
                {
                    Radius = ((CircleEntity)entity).Radius
                },
                _ => new(),
            };

            entityData.Id = entity.Id;
            entityData.Name = entity.Name;
            entityData.Type = entity.EntityType;
            entityData.Position = entity.transform.position;
            entityData.Rotation = entity.transform.rotation;
            entityData.GravityEnabled = entity.IsGravityEnabled;
            entityData.ColliderEnabled = entity.IsColliderEnabled;
            entityData.Layer = entity.Layer;
            entityData.Color = entity.CurrentColor;
            entityData.TextureSlotKey = entity.TextureSlotKey;
            entityData.ZDepth = entity.ZDepth;

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

        Debug.Log($"Load {data.Scene.Entities.Count} entities");

        foreach (var entityData in data.Scene.Entities)
        {
            SceneEntity entity;
            switch (entityData.Type)
            {
                case EntityType.Box:
                    BoxEntityData boxData = (BoxEntityData)entityData;
                    entity = AddBox(boxData.Position, boxData.Width, boxData.Height);
                    var box = (BoxEntity)entity;
                    
                    box.TextBox.Text = boxData.Text;
                    box.TextBox.Color = boxData.TextColor.ToUnityColor();
                    box.TextBox.Size = boxData.TextSize;
                    box.TextBox.HorizontalAlignment = boxData.TextHorizontalAlignment;
                    box.TextBox.VerticalAlignment = boxData.TextVerticalAlignment;
                    break;
                case EntityType.Circle:
                    CircleEntityData circleData = (CircleEntityData)entityData;
                    entity = AddCircle(circleData.Position, circleData.Radius);
                    break;
                default:
                    continue;
            }

            entity.Id = entityData.Id;
            entity.Name = entityData.Name;
            entity.Rotation = entityData.Rotation;
            entity.CurrentColor = entityData.Color;

            entity.ToggleCollider(entityData.ColliderEnabled);
            entity.ToggleGravity(entityData.GravityEnabled);
            entity.SetLayer(entityData.Layer);
            entity.SetTexture(entityData.TextureSlotKey);
            entity.ZDepth = entityData.ZDepth;
            
            ScriptSaveHandler.Load(entityData.Script, entity.Script);
        }

        SceneEntities.Sort((a, b) => a.Id.CompareTo(b.Id));
        _indexForId = SceneEntities.Count > 0 ? SceneEntities[^1].Id + 1 : 1;
    }
}