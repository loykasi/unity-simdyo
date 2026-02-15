using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Collider
{
    public abstract IEnumerable<Collider2D> Colliders { get; }
    public abstract bool IsTrigger { get; }
    public abstract void ToggleCollider(bool value);
    public abstract void Overlap(List<Collider2D> results);
    public abstract bool OverlapPoint(Vector2 point);
    public abstract void IgnoreCollision(Collider collider);
}