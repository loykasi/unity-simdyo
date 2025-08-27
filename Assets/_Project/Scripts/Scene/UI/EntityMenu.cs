using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EntityMenuController _controller;

    [Header("Menu")]
    [SerializeField] private TMP_InputField _positionXInput;
    [SerializeField] private TMP_InputField _positionYInput;
    [SerializeField] private TMP_InputField _angleInput;
    [SerializeField] private Toggle _gravityToggle;
    [SerializeField] private Toggle _colliderToggle;
    [SerializeField] private Image _buttonColor;

    [Header("Box")]
    [SerializeField] private GameObject _boxMenu;
    [SerializeField] private TMP_InputField _widthInput;
    [SerializeField] private TMP_InputField _heightInput;

    [Header("Circle")]
    [SerializeField] private GameObject _circleMenu;
    [SerializeField] private TMP_InputField _radiusInput;

    [Header("Collision layers")]
    [SerializeField] private CollisionLayerToggle _layerTogglePrefab;
    [SerializeField] private Transform _layerHolder;
    [SerializeField] private int _totalLayerPerRow;
    [SerializeField] private float _spaceBetweenLayer;
    private CollisionLayerToggle[] _collisionLayerToggles;

    private SceneEntity _entity;

    private void Awake()
    {
        CreateCollisionLayerMenu();

        _positionXInput.onEndEdit.AddListener(OnEditX);
        _positionYInput.onEndEdit.AddListener(OnEditY);
        _angleInput.onEndEdit.AddListener(OnEditAngle);
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
    }

    public void Init(SceneEntity entity)
    {
        _entity = entity;
        _positionXInput.SetTextWithoutNotify(entity.transform.position.x.ToString());
        _positionYInput.SetTextWithoutNotify(entity.transform.position.y.ToString());

        _angleInput.SetTextWithoutNotify(entity.transform.eulerAngles.z.ToString());

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

                _widthInput.text = ((BoxEntity)entity).Width.ToString();
                _heightInput.text = ((BoxEntity)entity).Height.ToString();
                break;
            case EntityType.Circle:
                _boxMenu.SetActive(false);
                _circleMenu.SetActive(true);

                _radiusInput.text = ((CircleEntity)entity).Radius.ToString();
                break;
        }
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

    public void ToggleLayer(CollisionLayer layer, bool isActive)
    {
        _controller.ToggleLayer(layer, isActive);
    }

    public void Delete()
    {
        _controller.Delete();
    }

    public void OnEditX(string value)
    {
        if (float.TryParse(value, out float result))
        {
            _positionXInput.SetTextWithoutNotify(result.ToString());
        }
        else
        {
            _positionXInput.SetTextWithoutNotify(_entity.Position.x.ToString());
            result = _entity.Position.x;
        }
        _controller.UpdatePosition(result, _entity.Position.y);
    }

    public void OnEditY(string value)
    {
        if (float.TryParse(value, out float result))
        {
            _positionYInput.SetTextWithoutNotify(result.ToString());
        }
        else
        {
            _positionYInput.SetTextWithoutNotify(_entity.Position.y.ToString());
            result = _entity.Position.y;
        }
        _controller.UpdatePosition(_entity.Position.x, result);
    }

    public void OnEditAngle(string value)
    {
        if (float.TryParse(value, out float result))
        {
            _positionYInput.SetTextWithoutNotify(result.ToString());
        }
        else
        {
            _positionYInput.SetTextWithoutNotify(_entity.Angle.ToString());
            result = _entity.Angle;
        }
        _controller.UpdateAngle(result);
    }

    public void ChooseTexture()
    {
        _controller.ChooseTexture();
    }
}