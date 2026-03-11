using System.Collections.Generic;
using UnityEngine;

public class CollisionLayerController : Singleton<CollisionLayerController>
{
    public List<SceneEntity> GetEntityList()
    {
        return ObjectManager.Instance.SceneEntities;
    }

    public void UpdateObjectLayer(MeshEntity meshEntity)
    {
        List<SceneEntity> entities = GetEntityList();

        MeshEntity a = meshEntity;

        for (int i = 0; i < entities.Count; i++)
        {
            if (entities[i] is not MeshEntity b)
            {
                continue;
            }

            if (a == b) continue;

            bool shouldCollide = (a.Layer & b.Layer) != 0;
            IgnoreCollision(a.Collider, b.Collider, isIgnore: !shouldCollide);
        }
    }

    public void UpdateAllObjectLayer()
    {
        List<SceneEntity> entities = GetEntityList();

        for (int i = 0; i < entities.Count; i++)
        {
            if (entities[i] is not MeshEntity a)
            {
                continue;
            }
            for (int j = i + 1; j < entities.Count; j++)
            {
                if (entities[j] is not MeshEntity b)
                {
                    continue;
                }

                bool shouldCollide = (a.Layer & b.Layer) != 0;
                IgnoreCollision(a.Collider, b.Collider, isIgnore: !shouldCollide);
            }
        }
    }

    private void IgnoreCollision(Collider colliderA, Collider colliderB, bool isIgnore)
    {
        foreach (Collider2D a in colliderA.Colliders)
        {
            foreach (Collider2D b in colliderB.Colliders)
            {
                Physics2D.IgnoreCollision(a, b, isIgnore);
            }
        }
    }
}