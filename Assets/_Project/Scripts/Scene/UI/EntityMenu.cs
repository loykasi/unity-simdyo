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

    [Header("Collision layers")]
    [SerializeField] private CollisionLayerToggle _layerTogglePrefab;
    [SerializeField] private Transform _layerHolder;
    [SerializeField] private int _totalLayerPerRow;
    [SerializeField] private float _spaceBetweenLayer;
    private CollisionLayerToggle[] _collisionLayerToggles;

    private void Awake()
    {
        CreateCollisionLayerMenu();
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
}