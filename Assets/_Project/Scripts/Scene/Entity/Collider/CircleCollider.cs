using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CircleCollider : Collider
{
    public override IEnumerable<Collider2D> Colliders => new Collider2D[]
    {
        Collider
    };

    public override bool IsTrigger => Collider.isTrigger;

    public CircleCollider2D Collider;

    public void SetRadius(float radius)
    {
        Collider.radius = radius;
    }

    public override void ToggleCollider(bool value)
    {
        Collider.isTrigger = !value;
    }

    public override void Overlap(List<Collider2D> results)
    {
        Collider.Overlap(results);
    }

    public override bool OverlapPoint(Vector2 point)
    {
        return Collider.OverlapPoint(point);
    }

    public override void IgnoreCollision(Collider collider)
    {
        
    }
}