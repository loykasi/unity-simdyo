using UnityEngine;

public interface ITool
{
    void Enable();
    void Disable();
    void OnUpdate(Vector3 mousePosition);
}