using System;

public enum ContextMenuActions
{
    None = 0,
    Clone,
    Delete,
    MoveToBack,
    MoveToFront,
    ResizeByTexture,
    Intersect,
    Subtract
}

[Flags]
public enum ContextMenuEntityTypes
{
    None    = 0,
    Box     = 1 << 0,
    Circle  = 1 << 1,
    Polygon = 1 << 2,
    Tracer  = 1 << 3,
    All     = ~0
}

public static class ContextMenuEntityTypeExtensions
{
    public static ContextMenuEntityTypes ToContextMenuType(this EntityType type)
    {
        return type switch
        {
            EntityType.Box => ContextMenuEntityTypes.Box,
            EntityType.Circle => ContextMenuEntityTypes.Circle,
            EntityType.Polygon => ContextMenuEntityTypes.Polygon,
            EntityType.Tracer => ContextMenuEntityTypes.Tracer,
            _ => ContextMenuEntityTypes.None
        };
    }

    public static bool Allow(this ContextMenuEntityTypes types, EntityType type)
    {
        return (types & type.ToContextMenuType()) != 0;
    }
}