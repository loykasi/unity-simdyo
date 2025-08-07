using UnityEngine;
using UnityEngine.UI;

public class EntityMenu : MonoBehaviour
{
    [SerializeField] private EntityMenuController _controller;
    [SerializeField] private Toggle _gravityToggle;
    [SerializeField] private Toggle _colliderToggle;

    public void Init(SceneEntity entity)
    {
        _gravityToggle.isOn = entity.IsGravityEnabled;
        _colliderToggle.isOn = entity.IsColliderEnabled;
    }

    public void ToggleGravity(bool value)
    {
        _controller.ToggleGravity(value);
    }

    public void ToggleCollider(bool value)
    {
        _controller.ToggleCollider(value);
    }
}