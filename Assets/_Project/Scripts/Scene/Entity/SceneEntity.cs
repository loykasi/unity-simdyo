using UnityEngine;
using UnityEngine.Events;
using Loykas.Scripting;
using UnityEngine.Rendering;
using System.Collections.Generic;

public abstract class SceneEntity : MonoBehaviour
{
    public UnityAction OnPropertyUpdated;

    public abstract EntityType EntityType { get; }
    public bool IsAddOnRuntime { get; set; } = false;

    public int Id;
    public string Name;
    public ScriptFlow Script;
    public SortingGroup SortingGroup;

    public virtual Vector3 Position
    {
        get => transform.position;
        set
        {
            transform.position = value;
            OnPropertyUpdated?.Invoke();
        }
    }

    public float Angle
    {
        get => transform.eulerAngles.z;
        set
        {
            transform.rotation = Quaternion.Euler(0f, 0f, value);
            OnPropertyUpdated?.Invoke();
        }
    }

    public virtual Quaternion Rotation
    {
        get => transform.rotation;
        set
        {
            transform.rotation = value;
            OnPropertyUpdated?.Invoke();
        }
    }

    public int ZDepth
    {
        get => SortingGroup.sortingOrder;
        set
        {
            SortingGroup.sortingOrder = value;
            OnPropertyUpdated?.Invoke();
        }
    }

    public abstract Bounds Bounds { get; }

    public List<SceneEntity> Relationships = new();

    protected TransformState _transformState;

    protected virtual void Awake()
    {
    }

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

    public void Delete()
    {
        Script.ResetState();

        foreach (SceneEntity entity in Relationships)
        {
            ObjectManager.Instance.DeleteEntity(entity);
        }

        Destroy(gameObject);
    }

    public virtual void OnSceneStart()
    {
        transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);

        _transformState = new(position, rotation, ZDepth);

        // if (Rigidbody.bodyType != RigidbodyType2D.Static)
        // {
        //     Rigidbody.linearVelocity = Velocity;   
        // }
    }

    public virtual void OnSceneStop()
    {
        transform.SetPositionAndRotation(_transformState.Position, _transformState.Rotation);
        ZDepth = _transformState.ZDepth;
    }

    public abstract void Select();
    public abstract void Deselect();

    // trigger hook
    public void OnStart()
    {
        Script.StartVS();
    }

    public void OnUpdate()
    {
        Script.UpdateVS();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Script.TriggerEvent(EventHook.OnTouched, collision);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Script.TriggerEvent(EventHook.OnTouched, collision);
    }

    public abstract SceneEntity CloneEntity();

    public virtual void CopyPropertyTo(SceneEntity entity)
    {
        entity.Rotation = Rotation;
        ScriptFlowClone.CloneScript(Script, entity.Script);
    }

    public abstract EntityData CreateSaveData();

    public void AddRelationship(SceneEntity entity)
    {
        Relationships.Add(entity);
    }

    public void RemoveRelationship(SceneEntity entity)
    {
        Relationships.Remove(entity);
    }
}