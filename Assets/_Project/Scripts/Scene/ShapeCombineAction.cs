using System.Collections.Generic;
using Clipper2Lib;
using Loykas.Scripting;
using UnityEngine;

public class ShapeCombineAction
{
    public void DoIntersection(MeshEntity entity)
    {
        if (entity == null)
        {
            return;
        }

        List<Collider2D> results = new();
        entity.Collider.Overlap(results);
        foreach (var collider in results)
        {
            if (collider is EdgeCollider2D)
            {
                continue;
            }
            Intersect(collider.GetComponent<MeshEntity>(), entity);
        }
    }

    private void Intersect(MeshEntity targetEntity, MeshEntity clipEntity)
    {
        PathsD targetPaths = targetEntity.ToPaths();
        PathsD clipPaths = clipEntity.ToPaths();

        PathsD solution = Clipper.Intersect(targetPaths, clipPaths, FillRule.EvenOdd, 8);

        List<Vector3> points = new();
        foreach (var path in solution)
        {
            points.Clear();
            foreach (var point in path)
            {
                points.Add(new Vector3((float)point.x, (float)point.y));
            }
            MeshEntity entity = ObjectManager.Instance.AddPolygon(points);
            if (entity != null)
            {
                entity.CurrentColor = targetEntity.CurrentColor;
                entity.IsColliderEnabled = targetEntity.IsColliderEnabled;
                entity.IsGravityEnabled = targetEntity.IsGravityEnabled;
                entity.SetLayer(targetEntity.Layer);
                entity.SetTexture(targetEntity.TextureSlotKey);
                ScriptFlowClone.CloneScript(targetEntity.Script, entity.Script);
            }
        }

        ObjectManager.Instance.DeleteEntity(targetEntity);
    }

    public void DoSubtract(MeshEntity entity)
    {
        if (entity == null)
        {
            return;
        }

        List<Collider2D> result = new();
        entity.Collider.Overlap(result);
        foreach (var collider in result)
        {
            // polygon has edge and polygon collider, we skip edge collider
            if (collider is EdgeCollider2D)
            {
                continue;
            }
            Subtract(collider.GetComponent<MeshEntity>(), entity);
        }
    }

    private void Subtract(MeshEntity targetEntity, MeshEntity clipEntity)
    {
        PathsD targetPaths = targetEntity.ToPaths();
        PathsD clipPaths = clipEntity.ToPaths();

        PathsD solution = Clipper.Difference(targetPaths, clipPaths, FillRule.EvenOdd, 8);

        List<Vector3> points = new();
        foreach (var path in solution)
        {
            points.Clear();
            foreach (var point in path)
            {
                points.Add(new Vector3((float)point.x, (float)point.y));
            }
            MeshEntity entity = ObjectManager.Instance.AddPolygon(points);
            if (entity != null)
            {
                entity.CurrentColor = targetEntity.CurrentColor;
                entity.IsColliderEnabled = targetEntity.IsColliderEnabled;
                entity.IsGravityEnabled = targetEntity.IsGravityEnabled;
                entity.SetLayer(targetEntity.Layer);
                entity.SetTexture(targetEntity.TextureSlotKey);
                entity.Friction = targetEntity.Friction;
                entity.Bounciness = targetEntity.Bounciness;
                ScriptFlowClone.CloneScript(targetEntity.Script, entity.Script);
            }
        }

        ObjectManager.Instance.DeleteEntity(targetEntity);
    }
}