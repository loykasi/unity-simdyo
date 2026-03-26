using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PolygonCollider : Collider
{
    public override bool IsTrigger => !EdgeCollider.enabled;

    public EdgeCollider2D EdgeCollider;
    public PolygonCollider2D AreaCollider;
    public PolygonCollider2D SolidAreaCollider;

    public override void Init()
    {
        Colliders = new Collider2D[]
        {
            EdgeCollider,
            AreaCollider
        };
    }

    public void SetPoints(Vector2[] points)
    {
        EdgeCollider.points = points;
        AreaCollider.points = points;
        SolidAreaCollider.points = points;
    }

    public Vector2 ClosestPoint(Vector2 point)
    {
        return EdgeCollider.ClosestPoint(point);
    }

    public override void SetPhysicsMaterial(PhysicsMaterial2D physicsMaterial)
    {
        EdgeCollider.sharedMaterial = physicsMaterial;
        AreaCollider.sharedMaterial = physicsMaterial;
        SolidAreaCollider.sharedMaterial = physicsMaterial;
    }

    public override void Overlap(List<Collider2D> results)
    {
        AreaCollider.Overlap(results);
    }

    public override void ToggleCollider(bool value)
    {
        EdgeCollider.enabled = value;
        SolidAreaCollider.enabled = value;
    }

    public override bool OverlapPoint(Vector2 point)
    {
        return AreaCollider.OverlapPoint(point);
    }
}