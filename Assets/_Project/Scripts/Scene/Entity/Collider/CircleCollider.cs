using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CircleCollider : Collider
{
    public override bool IsTrigger => Collider.isTrigger;

    public CircleCollider2D Collider;

    public override void Init()
    {
        Colliders = new Collider2D[]
        {
            Collider
        };
    }

    public void SetRadius(float radius)
    {
        Collider.radius = radius;
    }

    public override void SetPhysicsMaterial(PhysicsMaterial2D physicsMaterial)
    {
        Collider.sharedMaterial = physicsMaterial;
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
}