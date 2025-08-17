using UnityEngine;
using UnityEngine.UI;

public class EntityMenu : MonoBehaviour
{
    [SerializeField] private EntityMenuController _controller;
    [SerializeField] private Toggle _gravityToggle;
    [SerializeField] private Toggle _colliderToggle;
    [SerializeField] private Image _buttonColor;

    public void Init(SceneEntity entity)
    {
        _gravityToggle.isOn = entity.IsGravityEnabled;
        _colliderToggle.isOn = entity.IsColliderEnabled;
        _buttonColor.color = entity.UnityColor;
    }

    public void UpdateMenu(SceneEntity entity)
    {
        _buttonColor.color = entity.UnityColor;
    }

    public void ToggleGravity(bool value)
    {
        _controller.ToggleGravity(value);
    }

    public void ToggleCollider(bool value)
    {
        _controller.ToggleCollider(value);
    }

    public void OpenColorEdit()
    {
        _controller.OpenColorEdit();
    }

    public void OpenGraph()
    {
        ScriptGraph.Instance.TogglePanel();
    }
}