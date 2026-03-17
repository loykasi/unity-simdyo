using Clipper2Lib;
using UnityEngine;

public abstract class MeshEntity : SceneEntity
{
    public MeshFilter MeshFilter;
    public MeshRenderer Renderer;
    public abstract Collider Collider { get; }
    public Rigidbody2D Rigidbody;
    public CollisionLayer Layer;
    public string TextureSlotKey;
    public TextureSlot TextureSlot;
    public MeshWrapper Mesh;
    public Material Material;

    public Vector2 Velocity
    {
        get
        {
            return Rigidbody.linearVelocity;
        }
        set
        {
            // _velocity = value;
            Rigidbody.linearVelocity = value;
        }
    }
    // private Vector2 _velocity;

    public bool IsColliderEnabled
    {
        get => !Collider.IsTrigger;
        set
        {
            Collider.ToggleCollider(value);
        }
    }

    public bool IsGravityEnabled
    {
        get => Rigidbody.bodyType == RigidbodyType2D.Dynamic;
        set
        {
            Rigidbody.bodyType = value ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
        }   
    }

    public float Friction
    {
        get => _physicsMaterial.friction;
        set
        {
            _physicsMaterial.friction = value;
            Rigidbody.sharedMaterial = _physicsMaterial;
            OnPropertyUpdated?.Invoke();
        }
    }

    public float Bounciness
    {
        get => _physicsMaterial.bounciness;
        set
        {
            _physicsMaterial.bounciness = value;
            Rigidbody.sharedMaterial = _physicsMaterial;
            OnPropertyUpdated?.Invoke();
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
    private PhysicsMaterial2D _physicsMaterial;
    private readonly int _textureProperty = Shader.PropertyToID("_BaseMap");
    
    protected MeshState _meshState;

    protected override void Awake()
    {
        CollisionLayerController.Instance.UpdateObjectLayer(this);

        _physicsMaterial = new("PhysicsMaterial")
        {
            friction = 0.5f,
            bounciness = 0.5f
        };
        Rigidbody.sharedMaterial = _physicsMaterial;
    }

    public void SetLayer(int layer)
    {
        SetLayer((CollisionLayer)layer);
    }

    public void SetLayer(CollisionLayer layer)
    {
        Layer = layer;
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
        CollisionLayerController.Instance.UpdateObjectLayer(this);
    }

    public void SetColor(Color color)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);
        float a = color.a;

        CurrentColor = new ColorHSV(h, s, v, a);
    }

    private void UpdateColor()
    {
        Material.color = CurrentColor.ToUnityColor();
    }

    public virtual void SetTexture(string key)
    {
        if (TextureSlot != null)
        {
            TextureSlot.OnRemoved -= OnTextureSlotRemoved;
        }

        TextureSlotKey = key;
        TextureSlot = TextureController.Instance.GetTexture(key);
        if (TextureSlot != null)
        {
            Material.SetTexture(_textureProperty, TextureSlot.Texture);
            TextureSlot.OnRemoved += OnTextureSlotRemoved;
        }
    }

    private void OnTextureSlotRemoved()
    {
        TextureSlot.OnRemoved -= OnTextureSlotRemoved;
        Material.SetTexture(_textureProperty, null);
        TextureSlot = null;
    }

    public override void OnSceneStart()
    {
        base.OnSceneStart();
        _meshState = new()
        {
            ColorHSV = CurrentColor,
            ColliderEnabled = IsColliderEnabled,
            GravityEnabled = IsGravityEnabled,
            Velocity = Velocity,
            AngularVelocity = Rigidbody.angularVelocity,
            TextureSlotKey = TextureSlotKey
        };
    }

    public override void OnSceneStop()
    {
        base.OnSceneStop();
        CurrentColor = _meshState.ColorHSV;
        IsColliderEnabled = _meshState.ColliderEnabled;
        IsGravityEnabled = _meshState.GravityEnabled;
        Velocity = _meshState.Velocity;
        Rigidbody.angularVelocity = _meshState.AngularVelocity;

        SetTexture(_meshState.TextureSlotKey);
    }

    public override void CopyPropertyTo(SceneEntity entity)
    {
        if (entity is MeshEntity meshEntity)
        {
            meshEntity.CurrentColor = CurrentColor;
            meshEntity.IsColliderEnabled = IsColliderEnabled;
            meshEntity.IsGravityEnabled = IsGravityEnabled;
            meshEntity.SetLayer(Layer);
            meshEntity.SetTexture(TextureSlotKey);
            meshEntity.Friction = Friction;
            meshEntity.Bounciness = Bounciness;   
        }
        base.CopyPropertyTo(entity);
    }

    public abstract PathsD ToPaths();
}