using System.Collections.Generic;
using UnityEngine;

public class EntityGroup
{
    public List<SceneEntity> Entities = new();
    public Bounds Bounds;
    public Vector3 Center;

    public int Count => Entities.Count;
    public int MeshEntityCount;
    public int BoxCount;
    public int CircleCount;
    public int PolygonCount;
    public int TracerCount;

    public bool Contains(SceneEntity entity) => Entities.Contains(entity);
    
    public void Add(SceneEntity entity)
    {
        Entities.Add(entity);
        CalculateBounds();
        CalculateCenter();

        switch (entity)
        {
            case BoxEntity:
                MeshEntityCount++;
                BoxCount++;
                break;
            case CircleEntity:
                MeshEntityCount++;
                CircleCount++;
                break;
            case PolygonEntity:
                MeshEntityCount++;
                PolygonCount++;
                break;
            case TracerEntity:
                TracerCount++;
                break;
        }
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

        MeshEntityCount = 0;
        BoxCount = 0;
        CircleCount = 0;
        PolygonCount = 0;
        TracerCount = 0;
    }

    public void Update()
    {
        CalculateBounds();
        CalculateCenter();
    }
}