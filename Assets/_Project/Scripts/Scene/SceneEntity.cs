using UnityEngine;

public class SceneEntity : MonoBehaviour
{
    public MeshFilter MeshFilter;
    public MeshRenderer Renderer;
    public Collider2D Collider;
    public VisualScripting Script;

    private Vector3 _position;
    private Quaternion _rotation;

    public void OnSceneStart()
    {
        transform.GetPositionAndRotation(out _position, out _rotation);
        Script.OnSceneStart();
    }

    public void OnSceneStop()
    {
        transform.SetPositionAndRotation(_position, _rotation);
        Script.OnSceneStop();
    }
}