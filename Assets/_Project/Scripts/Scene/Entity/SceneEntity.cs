using UnityEngine;
using UnityEngine.Events;
using Loykas.Scripting;
public class SceneEntity : MonoBehaviour
{
    public UnityAction OnPropertyUpdated;

    public string ID;
    public virtual EntityType EntityType => EntityType.Polygon;

    public Vector3 Position
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

    public Quaternion Rotation
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

    public MeshFilter MeshFilter;
    public MeshRenderer Renderer;
    public Collider2D Collider;
    public Rigidbody2D Rigidbody;
    public ScriptFlow Script;
    public CollisionLayer Layer;
    public int TextureSlot = 0;

    public virtual Bounds Bounds => Renderer.bounds;

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

    public bool IsColliderEnabled { get; set; } = true;
    public bool IsGravityEnabled => Rigidbody.bodyType == RigidbodyType2D.Dynamic;

    private SceneEntityState _defaultState = new();

    private readonly int _textureProperty = Shader.PropertyToID("_BaseMap");

    private void Awake()
    {
        CollisionLayerController.Instance.UpdateObjectLayer(this);
    }

    public void AssignCollider(Collider2D collider)
    {
        Collider = collider;
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
            Layer ^= layer;
        }
        UpdateLayer();
    }

    public void OnSceneStart()
    {
        transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);

        _defaultState.Position = position;
        _defaultState.Rotation = rotation;
        _defaultState.ColliderEnabled = IsColliderEnabled;
        _defaultState.GravityEnabled = IsGravityEnabled;
        _defaultState.Velocity = Velocity;
        _defaultState.AngularVelocity = Rigidbody.angularVelocity;

        Collider.enabled = IsColliderEnabled;

        if (Rigidbody.bodyType != RigidbodyType2D.Static)
        {
            Rigidbody.linearVelocity = Velocity;   
        }

        Script.OnSceneStart();
    }

    public void OnSceneStop()
    {
        transform.SetPositionAndRotation(_defaultState.Position, _defaultState.Rotation);
        SetCollider(_defaultState.ColliderEnabled);
        SetGravity(_defaultState.GravityEnabled);
        if (_defaultState.GravityEnabled)
        {
            Velocity = _defaultState.Velocity;
            Rigidbody.angularVelocity = _defaultState.AngularVelocity;
        }

        Collider.enabled = true;

        Script.OnSceneStop();
    }

    public void SetCollider(bool value)
    {
        IsColliderEnabled = value;
    }

    public void SetGravity(bool value)
    {
        if (value)
        {
            Rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            Rigidbody.bodyType = RigidbodyType2D.Static;
        }
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

    public virtual void Select()
    {

    }

    public virtual void Deselect()
    {

    }

    public virtual void SetTexture(int slot, Texture2D texture)
    {
        TextureSlot = slot;
        Renderer.material.SetTexture(_textureProperty, texture);
        Debug.Log(TextureSlot);
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
}