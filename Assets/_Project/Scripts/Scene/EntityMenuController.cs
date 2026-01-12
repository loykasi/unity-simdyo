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
        _entity.ToggleGravity(value);
    }

    public void ToggleCollider(bool value)
    {
        _entity.ToggleCollider(value);
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

    public void UpdateName(string value)
    {
        ObjectManager.Instance.RenameEntity(_entity, value);
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
        TextureController.Instance.OpenMenu(_entity);
    }

    public void UpdateWidth(float width)
    {
        if (_entity.EntityType != EntityType.Box)
        {
            return;
        }

        var boxEntity = (BoxEntity)_entity;
        boxEntity.SetSize(width, boxEntity.Height);

        Physics2D.SyncTransforms();
    }

    public void UpdateHeight(float height)
    {
        if (_entity.EntityType != EntityType.Box)
        {
            return;
        }

        var boxEntity = (BoxEntity)_entity;
        boxEntity.SetSize(boxEntity.Width, height);

        Physics2D.SyncTransforms();
    }

    public void UpdateRadius(float radius)
    {
        if (_entity.EntityType != EntityType.Circle)
        {
            return;
        }

        Debug.Log("Set radius");

        ((CircleEntity)_entity).SetRadius(radius);

        Physics2D.SyncTransforms();
    }

    public void ResizeByTexture()
    {
        if (_entity.EntityType != EntityType.Box)
        {
            return;
        }

        var boxEntity = (BoxEntity)_entity;
        boxEntity.ResizeByTexture();

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