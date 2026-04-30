using System;
using UnityEngine;
using UnityEngine.UI;

public class MeshEntityMenu : BaseEntityMenu
{
    [Header("Properties")]
    [SerializeField] private Toggle _gravityToggle;
    [SerializeField] private MenuVectorInput _velocityInput;
    [SerializeField] private Toggle _colliderToggle;
    [SerializeField] private Image _buttonColor;
    [SerializeField] private MenuNumberInput _frictionInput;
    [SerializeField] private MenuNumberInput _bouncinessInput;

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

    private MeshEntity _entity;
    private BoxEntity _box;
    private CircleEntity _circle;

    public override void Setup()
    {
        CreateCollisionLayerMenu();
        
        _gravityToggle.onValueChanged.AddListener(ToggleGravity);
        _colliderToggle.onValueChanged.AddListener(ToggleCollider);
        _velocityInput.OnSubmit += OnVelocitySubmit;
        _radiusInput.OnSubmit += OnRadiusSubmit;
        _widthInput.OnSubmit += OnWidthSubmit;
        _heightInput.OnSubmit += OnHeightSubmit;
        _frictionInput.OnSubmit += OnFrictionSubmit;
        _bouncinessInput.OnSubmit += OnBouncinessSubmit;
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
        Debug.Log(_collisionLayer.sizeDelta);
    }

    public override void UpdateUI()
    {
        if (_entity == null)
        {
            return;
        }

        _velocityInput.SetValue(_entity.Velocity);
        _frictionInput.SetValue(_entity.Friction);
        _bouncinessInput.SetValue(_entity.Bounciness);

        _gravityToggle.isOn = _entity.IsGravityEnabled;
        _colliderToggle.isOn = _entity.IsColliderEnabled;
        _buttonColor.color = _entity.CurrentColor.ToUnityColor();;

        for (int i = 0; i < _collisionLayerToggles.Length; i++)
        {
            var toogle = _collisionLayerToggles[i];
            bool isSelected = (_entity.Layer & toogle.Layer) != 0;
            toogle.SetState(isSelected);
        }

        switch (_entity.EntityType)
        {
            case EntityType.Box:
                _boxMenu.SetActive(true);
                _circleMenu.SetActive(false);

                _widthInput.SetValue(_box.Width);
                _heightInput.SetValue(_box.Height);
                break;
            case EntityType.Circle:
                _boxMenu.SetActive(false);
                _circleMenu.SetActive(true);

                _radiusInput.SetValue(_circle.Radius);
                break;
            case EntityType.Polygon:
                _boxMenu.SetActive(false);
                _circleMenu.SetActive(false);
                break;
        }
    }

    public void ToggleLayer(CollisionLayer layer, bool isActive)
    {
        _entity.SetLayer(layer, isActive);
    }

    public override void AddEntity(SceneEntity entity)
    {
        if (entity is MeshEntity meshEntity)
        {
            _entity = meshEntity;
            _entity.OnPropertyUpdated += OnPropertyUpdated;
            switch (entity)
            {
                case BoxEntity boxEntity:
                    _box = boxEntity;
                    break;
                case CircleEntity circleEntity:
                    _circle = circleEntity;
                    break;
            }
        }
    }

    public override void Clear()
    {
        if (_entity != null)
        {
            _entity.OnPropertyUpdated -= OnPropertyUpdated;
            _entity = null;
            _box = null;
            _circle = null;
        }
    }

    public override void Show()
    {
        if (_entity != null)
        {
            _menuGameObject.SetActive(true);
        }
    }

    public override void Close()
    {
        Clear();
        _menuGameObject.SetActive(false);
    }

    private void OnPropertyUpdated()
    {
        UpdateUI();
    }

    private void OnVelocitySubmit(Vector3 value)
    {
        _entity.Velocity = new(value.x, value.y);
    }

    public void ToggleGravity(bool value)
    {
        _entity.IsGravityEnabled = value;
    }

    public void ToggleCollider(bool value)
    {
        _entity.IsColliderEnabled = value;
    }

    public void OpenColorEdit()
    {
        ColorPickerController.Instance.Open(_entity.CurrentColor, OnColorUpdated);
    }

    private void OnColorUpdated(ColorHSV color)
    {
        _entity.CurrentColor = color;
        _buttonColor.color = _entity.CurrentColor.ToUnityColor();
    }

    public void OnRadiusSubmit(float value)
    {
        _circle.SetRadius(value);
        Physics2D.SyncTransforms();
    }

    public void OnWidthSubmit(float value)
    {
        _box.SetWidth(value);
        Physics2D.SyncTransforms();
    }

    public void OnHeightSubmit(float value)
    {
        _box.SetHeight(value);
        Physics2D.SyncTransforms();
    }

    private void OnBouncinessSubmit(float value)
    {
        _entity.Bounciness = value;
    }

    private void OnFrictionSubmit(float value)
    {
        _entity.Friction = value;
    }

    public void OpenTextEditor()
    {
        TextBoxMenu.Instance.Open(_box.TextBox);
    }
}