using UnityEngine;

public class SceneEntity : MonoBehaviour
{
    public Collider2D Collider { get; set; }
    public VisualScripting VisualScripting { get; set; }

    private Vector3 _position;
    private Quaternion _rotation;

    public void OnSceneStart()
    {
        transform.GetPositionAndRotation(out _position, out _rotation);
        VisualScripting.OnSceneStart();
    }

    public void OnSceneStop()
    {
        transform.SetPositionAndRotation(_position, _rotation);
        VisualScripting.OnSceneStop();
    }
}