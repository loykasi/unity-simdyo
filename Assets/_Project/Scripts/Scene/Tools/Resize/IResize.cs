using UnityEngine;

public interface IResize
{
    void Init(SceneEntity entity);
    void BeginResize(BoundsHandleDirection direction);
    void Resize(BoundsHandleDirection direction, Vector3 mousePosition);
}