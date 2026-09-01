using System.Collections.Generic;
using UnityEngine;

public class EntityGroup
{
    public List<SceneEntity> Entities = new();
    public Bounds Bounds;
    public Vector3 Center;

    public int Count => Entities.Count;

    public bool Contains(SceneEntity entity) => Entities.Contains(entity);
    
    public void Add(SceneEntity entity)
    {
        Entities.Add(entity);
        CalculateBounds();
        CalculateCenter();
    }

    private void CalculateBounds()
    {
        Bounds = Entities[0].Bounds;
        for (int i = 1; i < Count; i++)
        {
            Bounds.Encapsulate(Entities[i].Bounds);
        }
    }

    private void CalculateCenter()
    {
        Center = Vector3.zero;
        for (int i = 0; i < Count; i++)
        {
            Center += Entities[i].Position;
        }
        Center /= Count;
    }

    public void Deselect()
    {
        foreach (SceneEntity entity in Entities)
        {
            entity.Deselect();
        }
        Entities.Clear();
    }

    public void Update()
    {
        CalculateBounds();
        CalculateCenter();
    }
}