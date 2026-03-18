using UnityEngine;

public class TracerEntity : SceneEntity
{
    public override EntityType EntityType => EntityType.Tracer;
    public override Bounds Bounds => throw new System.NotImplementedException();

    public SpriteRenderer SpriteRenderer;
    public TrailRenderer TrailRenderer;

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
        }
    }
    private float _diameter;

    public override SceneEntity CloneEntity()
    {
        throw new System.NotImplementedException();
    }

    public override EntityData CreateSaveData()
    {
        throw new System.NotImplementedException();
    }

    public override void Deselect()
    {
        throw new System.NotImplementedException();
    }

    public override void Select()
    {
        throw new System.NotImplementedException();
    }
}