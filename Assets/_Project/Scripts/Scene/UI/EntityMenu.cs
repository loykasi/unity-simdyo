using System;
using Loykas.Scripting;
using UnityEngine;
using UnityEngine.UI;

public class EntityMenu : MonoBehaviour
{
    public bool IsOpened;

    [Header("Entity")]
    [SerializeField] private StringInput _idInput;
    [SerializeField] private StringInput _nameInput;
    [SerializeField] private MenuVectorInput _positionInput;
    [SerializeField] private MenuNumberInput _angleInput;

    [Header("Tracer Properties")]
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

    [Header("Selection")]
    [SerializeField] private MenuNumberInput _depthInput;

    private MeshEntity _entity;

    private void Awake()
    {
        CreateCollisionLayerMenu();

        _nameInput.OnSubmit += OnNameSubmit;
        _positionInput.OnSubmit += OnPositionSubmit;
        _angleInput.OnSubmit += OnAngleSubmit;
        _velocityInput.OnSubmit += OnVelocitySubmit;
        _radiusInput.OnSubmit += OnRadiusSubmit;
        _widthInput.OnSubmit += OnWidthSubmit;
        _heightInput.OnSubmit += OnHeightSubmit;
        _depthInput.OnSubmit += OnDepthSubmit;
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
    }

    public void Open(MeshEntity entity)
    {
        gameObject.SetActive(true);
        _entity = entity;
        _entity.OnPropertyUpdated += OnPropertyUpdated;

        Init(entity);
        IsOpened = true;
    }

    public void Close()
    {
        if (IsOpened)
        {
            gameObject.SetActive(false);
            _entity.OnPropertyUpdated -= OnPropertyUpdated;
            _entity = null;
            IsOpened = false;
        }
    }

    private void OnPropertyUpdated()
    {
        Init(_entity);
    }

    private void Init(MeshEntity entity)
    {
        _idInput.SetValue(entity.Id.ToString());
        _nameInput.SetValue(entity.Name);
        _positionInput.SetValue(entity.Position);
        _angleInput.SetValue(entity.Angle);
        _velocityInput.SetValue(entity.Velocity);
        _frictionInput.SetValue(entity.Friction);
        _bouncinessInput.SetValue(entity.Bounciness);

        _gravityToggle.isOn = entity.IsGravityEnabled;
        _colliderToggle.isOn = entity.IsColliderEnabled;
        _buttonColor.color = entity.CurrentColor.ToUnityColor();;

        _depthInput.SetValue(entity.ZDepth);

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

                _widthInput.SetValue(boxEntity.Width);
                _heightInput.SetValue(boxEntity.Height);
                break;
            case EntityType.Circle:
                _boxMenu.SetActive(false);
                _circleMenu.SetActive(true);

                CircleEntity circleEntity = (CircleEntity)entity;
                _radiusInput.SetValue(circleEntity.Radius);
                break;
            case EntityType.Polygon:
                _boxMenu.SetActive(false);
                _circleMenu.SetActive(false);
                break;
        }
    }

    public void OnNameSubmit(object value)
    {
        ObjectManager.Instance.RenameEntity(_entity, (string)value);
    }

    private void OnPositionSubmit(Vector3 value)
    {
        _entity.Position = new Vector3(value.x, value.y);
        Physics2D.SyncTransforms();
    }

    private void OnAngleSubmit(float value)
    {
        _entity.Angle = value;
        Physics2D.SyncTransforms();
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
        if (_entity is CircleEntity circleEntity)
        {
            circleEntity.SetRadius(value);
            Physics2D.SyncTransforms();
        }
    }

    public void OnWidthSubmit(float value)
    {
        if (_entity is BoxEntity boxEntity)
        {
            boxEntity.SetSize(value, boxEntity.Height);
            Physics2D.SyncTransforms();
        }
    }

    public void OnHeightSubmit(float value)
    {
        if (_entity is BoxEntity boxEntity)
        {
            boxEntity.SetSize(boxEntity.Width, value);
            Physics2D.SyncTransforms();
        }
    }

    public void OnDepthSubmit(float value)
    {
        _entity.ZDepth = (int)value;
    }

    public void ResizeByTexture()
    {
        if (_entity is BoxEntity boxEntity)
        {
            boxEntity.ResizeByTexture();
            Physics2D.SyncTransforms();
        }
    }

    private void OnBouncinessSubmit(float value)
    {
        _entity.Bounciness = value;
    }

    private void OnFrictionSubmit(float value)
    {
        _entity.Friction = value;
    }

    public void OpenGraph()
    {
        SceneEntity selected = ObjectManager.Instance.SelectedObject;
        ScriptGraph.Instance.TogglePanel(selected.Script);
    }

    public void ToggleLayer(CollisionLayer layer, bool isActive)
    {
        _entity.SetLayer(layer, isActive);
    }

    public void ChooseTexture()
    {
        TextureController.Instance.OpenMenu(_entity);
    }

    public void OpenTextEditor()
    {
        if (_entity is BoxEntity boxEntity)
        {
            TextBoxMenu.Instance.Open(boxEntity.TextBox);
        }
    }
}