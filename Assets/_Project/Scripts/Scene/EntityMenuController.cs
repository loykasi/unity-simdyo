using System;
using System.IO;
using SFB;
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

    public void UpdatePosition(float x, float y)
    {
        _entity.Position = new Vector3(x, y);
    }

    public void UpdateAngle(float angle)
    {
        _entity.Angle = angle;
    }

    public void ChooseTexture()
    {
        if (AssetController.Instance.TryChooseTextureFile(out Texture2D texture))
        {
            _entity.SetTexture(texture);
        }
    }
}