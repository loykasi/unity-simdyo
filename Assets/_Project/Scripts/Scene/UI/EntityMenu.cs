using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EntityMenuController _controller;

    [Header("Menu")]
    [SerializeField] private MenuVectorInput _positionInput;
    [SerializeField] private MenuNumberInput _angleInput;
    [SerializeField] private Toggle _gravityToggle;
    [SerializeField] private Toggle _colliderToggle;
    [SerializeField] private Image _buttonColor;

    [Header("Box")]
    [SerializeField] private GameObject _boxMenu;
    [SerializeField] private MenuNumberInput _widthInput;
    [SerializeField] private MenuNumberInput _heightInput;

    [Header("Circle")]
    [SerializeField] private GameObject _circleMenu;
    [SerializeField] private MenuNumberInput _radiusInput;

    [Header("Collision layers")]
    [SerializeField] private RectTransform _collisionLayer;
    [SerializeField] private CollisionLayerToggle _layerTogglePrefab;
    [SerializeField] private Transform _layerHolder;
    [SerializeField] private int _totalLayerPerRow;
    [SerializeField] private float _spaceBetweenLayer;
    private CollisionLayerToggle[] _collisionLayerToggles;

    private void Awake()
    {
        CreateCollisionLayerMenu();

        _positionInput.OnSubmit += OnPositionSubmit;
        _angleInput.OnSubmit += OnAngleSubmit;
    }

    private void CreateCollisionLayerMenu()
    {
        var layers = Enum.GetValues(typeof(CollisionLayer));
        _collisionLayerToggles = new CollisionLayerToggle[layers.Length];

        int rowItemIndex = 0;
        float row = 0;
        int i = 0;

        foreach (CollisionLayer layer in layers)
        {
            CollisionLayerToggle layerToggle = Instantiate(_layerTogglePrefab, _layerHolder);

            Vector3 position = new(rowItemIndex * (layerToggle.Size + _spaceBetweenLayer), row);

            layerToggle.Init(position, layer, ToggleLayer);

            rowItemIndex++;

            if (rowItemIndex == _totalLayerPerRow)
            {
                row = row - layerToggle.Size - _spaceBetweenLayer;
                rowItemIndex = 0;
            }

            _collisionLayerToggles[i] = layerToggle;
            i++;
        }

        int totalRow = layers.Length / _totalLayerPerRow;
        float height = totalRow * 30f + totalRow * _spaceBetweenLayer;
        _collisionLayer.sizeDelta = new(_collisionLayer.sizeDelta.x, _collisionLayer.sizeDelta.y + height);
    }

    public void Init(SceneEntity entity)
    {
        // _positionInput.SetValue(entity.Position);
        // _angleInput.SetValue(entity.Angle);

        _gravityToggle.isOn = entity.IsGravityEnabled;
        _colliderToggle.isOn = entity.IsColliderEnabled;
        _buttonColor.color = entity.UnityColor;

        for (int i = 0; i < _collisionLayerToggles.Length; i++)
        {
            var toogle = _collisionLayerToggles[i];
            bool isSelected = (entity.Layer & toogle.Layer) != 0;
            toogle.SetState(isSelected);
        }

        switch (entity.EntityType)
        {
            case EntityType.Box:
                _boxMenu.SetActive(true);
                _circleMenu.SetActive(false);
                BoxEntity boxEntity = (BoxEntity)entity;

                // _widthInput.SetValue(boxEntity.Width);
                // _heightInput.SetValue(boxEntity.Height);
                break;
            case EntityType.Circle:
                _boxMenu.SetActive(false);
                _circleMenu.SetActive(true);

                CircleEntity circleEntity = (CircleEntity)entity;
                // _radiusInput.SetValue(circleEntity.Radius);
                break;
        }
    }

    public void UpdateMenu(SceneEntity entity)
    {
        _buttonColor.color = entity.UnityColor;
    }

    private void OnPositionSubmit(Vector3 value)
    {
        _controller.UpdatePosition(value.x, value.y);
    }

    private void OnAngleSubmit(float value)
    {
        _controller.UpdateAngle(value);
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

    public void ToggleLayer(CollisionLayer layer, bool isActive)
    {
        _controller.ToggleLayer(layer, isActive);
    }

    public void Delete()
    {
        _controller.Delete();
    }

    public void ChooseTexture()
    {
        _controller.ChooseTexture();
    }
}