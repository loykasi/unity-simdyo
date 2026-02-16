using UnityEngine;
using UnityEngine.Events;
using Loykas.Scripting;
using UnityEngine.Rendering;
using Clipper2Lib;

public abstract class SceneEntity : MonoBehaviour
{
    public UnityAction OnPropertyUpdated;

    public abstract EntityType EntityType { get; }

    // Marked as true if added during running scene
    public bool IsAddOnRuntime { get; set; } = false;

    public int Id;
    public MeshFilter MeshFilter;
    public MeshRenderer Renderer;
    // public Collider2D Collider;
    public abstract Collider Collider { get; }
    public Rigidbody2D Rigidbody;
    public ScriptFlow Script;
    public CollisionLayer Layer;
    public SortingGroup SortingGroup;
    public string TextureSlotKey;
    public Texture2D Texture;
    
    public string Name
    {
        get => gameObject.name;
        set
        {
            gameObject.name = value;
            OnUpdateProperty();
        }
    }

    public virtual Vector3 Position
    {
        get => transform.position;
        set
        {
            transform.position = value;
            OnUpdateProperty();
        }
    }

    public float Angle
    {
        get => transform.eulerAngles.z;
        set
        {
            transform.rotation = Quaternion.Euler(0f, 0f, value);
            OnUpdateProperty();
        }
    }

    public virtual Quaternion Rotation
    {
        get => transform.rotation;
        set
        {
            transform.rotation = value;
            OnUpdateProperty();
        }
    }

    public Vector2 Velocity
    {
        get
        {
            return _velocity;
        }
        set
        {
            _velocity = value;
            Rigidbody.linearVelocity = value;
        }
    }
    private Vector2 _velocity;

    public int ZDepth
    {
        get => SortingGroup.sortingOrder;
        set
        {
            SortingGroup.sortingOrder = value;
            OnUpdateProperty();
        }
    }

    public float Friction
    {
        get => _physicsMaterial.friction;
        set
        {
            _physicsMaterial.friction = value;
            Rigidbody.sharedMaterial = _physicsMaterial;
            OnUpdateProperty();
        }
    }

    public float Bounciness
    {
        get => _physicsMaterial.bounciness;
        set
        {
            _physicsMaterial.bounciness = value;
            Rigidbody.sharedMaterial = _physicsMaterial;
            OnUpdateProperty();
        }
    }

    public ColorHSV CurrentColor
    {
        get
        {
            return _currentColor;
        }
        set
        {
            _currentColor = value;
            UpdateColor();
        }
    }
    private ColorHSV _currentColor = new();

    public Color UnityColor
    {
        get
        {
            Color color = Color.HSVToRGB(_currentColor.H, _currentColor.S, _currentColor.V);
            color.a = _currentColor.A;
            return color;
        }
    }

    public bool IsColliderEnabled => !Collider.IsTrigger;
    public bool IsGravityEnabled => Rigidbody.bodyType == RigidbodyType2D.Dynamic;
    public virtual Bounds Bounds => Renderer.bounds;

    private PhysicsMaterial2D _physicsMaterial;

    protected SceneEntityState _defaultState = new();
    private readonly int _textureProperty = Shader.PropertyToID("_BaseMap");

    private void Awake()
    {
        CollisionLayerController.Instance.UpdateObjectLayer(this);

        _physicsMaterial = new("PhysicsMaterial")
        {
            friction = 0.5f,
            bounciness = 0.5f
        };
        Rigidbody.sharedMaterial = _physicsMaterial;
        // Collider.SetPhysicsMaterial(_physicsMaterial);
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

    public void SetLayer(CollisionLayer layer)
    {
        Layer = layer;
        CollisionLayerController.Instance.UpdateObjectLayer(this);
    }

    [ContextMenu("Update Layer")]
    public void UpdateLayer()
    {
        CollisionLayerController.Instance.UpdateObjectLayer(this);
    }

    public void SetLayer(CollisionLayer layer, bool isActive)
    {
        if (isActive)
        {
            Layer |= layer;
        }
        else
        {
            Layer &= ~layer;
        }
        UpdateLayer();
    }

    public void SetLayer(int layer)
    {
        SetLayer((CollisionLayer)layer);
    }

    public virtual void OnSceneStart()
    {
        transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);

        _defaultState.Position = position;
        _defaultState.Rotation = rotation;
        _defaultState.ColliderEnabled = IsColliderEnabled;
        _defaultState.GravityEnabled = IsGravityEnabled;
        _defaultState.Velocity = Velocity;
        _defaultState.AngularVelocity = Rigidbody.angularVelocity;
        _defaultState.TextureSlotKey = TextureSlotKey;

        if (Rigidbody.bodyType != RigidbodyType2D.Static)
        {
            Rigidbody.linearVelocity = Velocity;   
        }

        // Script.OnSceneStart();
    }

    public virtual void OnSceneStop()
    {
        transform.SetPositionAndRotation(_defaultState.Position, _defaultState.Rotation);
        ToggleCollider(_defaultState.ColliderEnabled);
        ToggleGravity(_defaultState.GravityEnabled);
        if (_defaultState.GravityEnabled)
        {
            Velocity = _defaultState.Velocity;
            Rigidbody.angularVelocity = _defaultState.AngularVelocity;
        }

        SetTexture(_defaultState.TextureSlotKey);

        // Script.OnSceneStop();
    }

    public void ToggleCollider(bool value)
    {
        Collider.ToggleCollider(value);
    }

    public void ToggleGravity(bool value)
    {
        Rigidbody.bodyType = value ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
    }

    public void SetColor(Color color)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);
        float a = color.a;

        CurrentColor = new ColorHSV(h, s, v, a);
    }

    private void UpdateColor()
    {
        Color color = Color.HSVToRGB(CurrentColor.H, CurrentColor.S, CurrentColor.V);
        color.a = CurrentColor.A;
        Renderer.material.color = color;
    }

    public void OnUpdateProperty()
    {
        OnPropertyUpdated?.Invoke();
    }

    public abstract void Select();
    public abstract void Deselect();

    public virtual void SetTexture(string key)
    {
        TextureSlotKey = key;
        Texture = TextureController.Instance.GetTexture(key);
        Renderer.material.SetTexture(_textureProperty, Texture);
    }

    // trigger hook
    public void OnStart()
    {
        Script.StartVS();
    }

    public void OnUpdate()
    {
        _velocity = Rigidbody.linearVelocity;
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
        entity.CurrentColor = CurrentColor;

        entity.ToggleCollider(IsColliderEnabled);
        entity.ToggleGravity(IsGravityEnabled);
        entity.SetLayer(Layer);
        entity.SetTexture(TextureSlotKey);

        ScriptFlowClone.CloneScript(Script, entity.Script);
    }

    public abstract PathsD ToPaths();
}