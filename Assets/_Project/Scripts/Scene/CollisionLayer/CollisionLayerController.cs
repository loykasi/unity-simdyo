using System.Collections.Generic;
using UnityEngine;

public class CollisionLayerController : Singleton<CollisionLayerController>
{
    public List<SceneEntity> GetEntityList()
    {
        return ObjectManager.Instance.SceneEntities;
    }

    public void UpdateObjectLayer(SceneEntity sceneEntity)
    {
        List<SceneEntity> entities = GetEntityList();

        SceneEntity a = sceneEntity;

        for (int i = 0; i < entities.Count; i++)
        {
            SceneEntity b = entities[i];

            if (a == b) continue;

            bool shouldCollide = (a.Layer & b.Layer) != 0;

            // Physics2D.IgnoreCollision(a.Collider, b.Collider, !shouldCollide);
            IgnoreCollision(a.Collider, b.Collider, isIgnore: !shouldCollide);
        }
    }

    public void UpdateAllObjectLayer()
    {
        List<SceneEntity> entities = GetEntityList();

        for (int i = 0; i < entities.Count; i++)
        {
            for (int j = i + 1; j < entities.Count; j++)
            {
                SceneEntity a = entities[i];
                SceneEntity b = entities[j];

                bool shouldCollide = (a.Layer & b.Layer) != 0;

                // Physics2D.IgnoreCollision(a.Collider, b.Collider, shouldCollide);
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