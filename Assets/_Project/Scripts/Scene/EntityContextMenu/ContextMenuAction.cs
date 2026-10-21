using UnityEngine;

public abstract class ContextMenuAction
{
    public abstract ContextMenuActions Type { get; }
    public virtual ContextMenuEntityTypes AllowedEntityTypes => ContextMenuEntityTypes.All;
    public virtual bool IsRequireSingle => false;

    public bool IsAvailable(EntityGroup group)
    {
        if (group == null || group.Count == 0)
        {
            return false;
        }

        if (group.Count > 1 && IsRequireSingle)
        {
            return false;
        }

        foreach (SceneEntity entity in group.Entities)
        {
            if (!AllowedEntityTypes.Allow(entity.EntityType))
            {
                return false;
            }
        }

        return true;
    }

    public abstract void Execute(EntityGroup group);
}

public class CloneContextMenuAction : ContextMenuAction
{
    public override ContextMenuActions Type => ContextMenuActions.Clone;

    public override void Execute(EntityGroup group)
    {
        Vector3 offset = new(group.Bounds.size.x, 0f, 0f);

        for (int i = group.Count - 1; i >= 0; i--)
        {
            SceneEntity entity = group.Entities[i];
            SceneEntity clone = entity.CloneEntity();
            Vector3 position = clone.Position + offset;
            clone.Position = position;

            ObjectManager.Instance.Deselect(entity);
            ObjectManager.Instance.Select(clone);
        }

        Physics2D.SyncTransforms();
    }
}

public class DeleteContextMenuAction : ContextMenuAction
{
    public override ContextMenuActions Type => ContextMenuActions.Delete;

    public override void Execute(EntityGroup group)
    {
        for (int i = group.Count - 1; i >= 0; i--)
        {
            SceneEntity entity = group.Entities[i];
            ObjectManager.Instance.DeleteEntity(entity);
        }
    }
}

public class MoveToBackContextMenuAction : ContextMenuAction
{
    public override ContextMenuActions Type => ContextMenuActions.MoveToBack;

    public override void Execute(EntityGroup group)
    {
        foreach (SceneEntity entity in group.Entities)
        {
            ObjectManager.Instance.MoveToBack(entity);
        }
    }
}

public class MoveToFrontContextMenuAction : ContextMenuAction
{
    public override ContextMenuActions Type => ContextMenuActions.MoveToFront;

    public override void Execute(EntityGroup group)
    {
        foreach (SceneEntity entity in group.Entities)
        {
            ObjectManager.Instance.MoveToFront(entity);
        }
    }
}

public class IntersectContextMenuAction : ContextMenuAction
{
    public override ContextMenuActions Type => ContextMenuActions.Intersect;
    public override ContextMenuEntityTypes AllowedEntityTypes =>
        ContextMenuEntityTypes.Box |
        ContextMenuEntityTypes.Circle |
        ContextMenuEntityTypes.Polygon;
    public override bool IsRequireSingle => true;

    public override void Execute(EntityGroup group)
    {
        ObjectManager.Instance.DoIntersection(group.Entities[0]);
    }
}

public class SubtractContextMenuAction : ContextMenuAction
{
    public override ContextMenuActions Type => ContextMenuActions.Subtract;
    public override ContextMenuEntityTypes AllowedEntityTypes =>
        ContextMenuEntityTypes.Box |
        ContextMenuEntityTypes.Circle |
        ContextMenuEntityTypes.Polygon;
    public override bool IsRequireSingle => true;

    public override void Execute(EntityGroup group)
    {
        ObjectManager.Instance.DoSubtract(group.Entities[0]);
    }
}

public class ResizeByTextureContextMenuAction : ContextMenuAction
{
    public override ContextMenuActions Type => ContextMenuActions.ResizeByTexture;
    public override ContextMenuEntityTypes AllowedEntityTypes =>
        ContextMenuEntityTypes.Box;
    public override bool IsRequireSingle => true;

    public override void Execute(EntityGroup group)
    {
        if (group.Entities[0] is BoxEntity boxEntity)
        {
            boxEntity.ResizeByTexture();
            Physics2D.SyncTransforms();
        }
    }
}