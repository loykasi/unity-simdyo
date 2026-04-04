using UnityEngine;

public class TracerEntity : SceneEntity
{
    public override EntityType EntityType => EntityType.Tracer;
    public override Bounds Bounds => _bounds;
    public Bounds _bounds = new();

    public SpriteRenderer SpriteRenderer;
    public TrailRenderer TrailRenderer;
    public int ParentId;
    public SceneEntity Parent;

    public float Time
    {
        get => TrailRenderer.time;
        set
        {
            TrailRenderer.time = value;
        }
    }

    public float Diameter
    {
        get => _diameter;
        set
        {
            _diameter = value;
            TrailRenderer.startWidth = _diameter;
            SpriteRenderer.size = new(_diameter, _diameter);
            _interactionCircle.radius = _diameter / 2f;
            _highlight.SetRadius(_diameter / 2f);
        }
    }
    private float _diameter;

    public ColorHSV Color
    {
        get => _color;
        set
        {
            _color = value;
            Color color = _color.ToUnityColor();
            SpriteRenderer.color = color;
            TrailRenderer.startColor = color;

            color.a = 0f;
            TrailRenderer.endColor = color;
        }
    }
    private ColorHSV _color = ColorHSV.Default;

    [SerializeField] private CircleCollider2D _interactionCircle;
    [SerializeField] private CircleHighlight _highlight;

    public void Init()
    {
        _highlight.Create(Diameter);
    }

    public override void OnSceneStart()
    {
        TrailRenderer.emitting = true;
        TrailRenderer.Clear();
    }

    public override void OnSceneStop()
    {
        TrailRenderer.emitting = false;
        TrailRenderer.Clear();
    }

    public override SceneEntity CloneEntity()
    {
        throw new System.NotImplementedException();
    }

    public override void Deselect()
    {
        _highlight.Disable();
    }

    public override void Select()
    {
        _highlight.Enable();
        _highlight.SetRadius(Diameter / 2f);
    }

    public void AutoAttachToMeshEntity()
    {
        bool result = ObjectManager.Instance.TryGetSceneEntityBelow(Position, ZDepth, out SceneEntity entity);
        if (result)
        {
            if (Parent != null)
            {
                Parent.RemoveRelationship(this);
            }

            ParentId = entity.Id;
            Parent = entity;
            transform.SetParent(entity.transform);
            entity.AddRelationship(this);
        }
    }

    public override void LoadRelationship()
    {
        Parent = ObjectManager.Instance.GetEntityById(ParentId);
        if (Parent == null)
        {
            ParentId = 0;
        }
        else
        {
            transform.SetParent(Parent.transform);
            Parent.AddRelationship(this);
        }
    }

    public override EntityData CreateSaveData()
    {
        return new TracerEntityData
        {
            Id = Id,
            Name = Name,
            Type = EntityType,
            Position = transform.position,
            Rotation = transform.rotation,
            ZDepth = ZDepth,
            Time = Time,
            Diameter = Diameter,
            Color = Color,
            ParentId = ParentId,
        };
    }
}