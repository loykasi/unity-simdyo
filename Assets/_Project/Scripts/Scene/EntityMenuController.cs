using UnityEngine;

public class EntityMenuController : MonoBehaviour
{
    [SerializeField] private EntityMenu _menu;
    private SceneEntity _entity;

    private void OnEnable()
    {
        ObjectManager.Instance.OnObjectSelected += OnObjectSelected;
        ObjectManager.Instance.OnObjectDeselected += OnObjectDeselected;
    }

    private void OnDisable()
    {
        if (ObjectManager.Instance)
        {
            ObjectManager.Instance.OnObjectSelected -= OnObjectSelected;
            ObjectManager.Instance.OnObjectDeselected -= OnObjectDeselected;
        }
    }

    private void OnObjectDeselected()
    {
        if (_entity != null)
        {
            _entity.OnPropertyUpdated -= OnPropertyUpdated;
            _entity = null;
        }

        _menu.gameObject.SetActive(false);
    }

    private void OnObjectSelected(SceneEntity entity)
    {
        if (entity != null)
        {
            _menu.gameObject.SetActive(true);
            _entity = entity;
            _entity.OnPropertyUpdated += OnPropertyUpdated;

            _menu.Init(_entity);
        }
    }

    private void OnPropertyUpdated()
    {
        _menu.Init(_entity);
    }

    public void ToggleGravity(bool value)
    {
        _entity.SetGravity(value);
    }

    public void ToggleCollider(bool value)
    {
        _entity.SetCollider(value);
    }

    public void OpenColorEdit()
    {
        ColorPickerController.Instance.Open(_entity.CurrentColor, OnColorUpdated);
    }

    private void OnColorUpdated(ColorHSV color)
    {
        _entity.CurrentColor = color;
        _menu.UpdateMenu(_entity);
    }

    public void ToggleLayer(CollisionLayer layer, bool isActive)
    {
        _entity.SetLayer(layer, isActive);
    }

    public void Delete()
    {
        ObjectManager.Instance.DeleteEntity(_entity);
        _entity = null;
        _menu.gameObject.SetActive(false);
    }

    public void UpdatePosition(float x, float y)
    {
        _entity.Position = new Vector3(x, y);
        Physics2D.SyncTransforms();
    }

    public void UpdateAngle(float angle)
    {
        _entity.Angle = angle;
        Physics2D.SyncTransforms();
    }

    public void UpdateVelocity(float x, float y)
    {
        _entity.Velocity = new Vector2(x, y);
    }

    public void ChooseTexture()
    {
        TextureMenu.Instance.OpenTextureMenu(_entity);
    }

    public void UpdateSize(float width, float height)
    {
        if (_entity.EntityType != EntityType.Box)
        {
            return;
        }

        ((BoxEntity)_entity).Width = width;
        ((BoxEntity)_entity).Height = height;

        Physics2D.SyncTransforms();
    }

    public void UpdateRadius(float radius)
    {
        if (_entity.EntityType != EntityType.Circle)
        {
            return;
        }

        ((CircleEntity)_entity).Radius = radius;

        Physics2D.SyncTransforms();
    }

    public void MoveToBack()
    {
        ObjectManager.Instance.MoveToBack(_entity);
    }

    public void MoveToFront()
    {
        ObjectManager.Instance.MoveToFront(_entity);
    }
}