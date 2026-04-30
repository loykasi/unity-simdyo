using System.Collections.Generic;
using Loykas.Scripting;
using UnityEngine;

public class EntityMenu : MonoBehaviour
{

    [Header("Common")]
    [SerializeField] private StringInput _idInput;
    [SerializeField] private StringInput _nameInput;
    [SerializeField] private MenuVectorInput _positionInput;
    [SerializeField] private MenuNumberInput _angleInput;
    [SerializeField] private MenuNumberInput _depthInput;

    [Header("Type")]
    [SerializeField] private BaseEntityMenu[] _entityMenus;

    private SceneEntity _entity;

    public void Setup()
    {
        _nameInput.OnSubmit += OnNameSubmit;
        _positionInput.OnSubmit += OnPositionSubmit;
        _angleInput.OnSubmit += OnAngleSubmit;
        _depthInput.OnSubmit += OnDepthSubmit;

        foreach (BaseEntityMenu menu in _entityMenus)
        {
            menu.Setup();
        }
    }

    public void Open(SceneEntity entity)
    {
        Clear();

        _entity = entity;
        _entity.OnPropertyUpdated += OnPropertyUpdated;
        foreach (BaseEntityMenu menu in _entityMenus)
        {
            menu.Clear();
            menu.AddEntity(entity);
            menu.Show();
        }

        UpdateUI();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        Clear();
        foreach (BaseEntityMenu menu in _entityMenus)
        {
            menu.Close();
        }
        gameObject.SetActive(false);
    }

    private void Clear()
    {
        if (_entity != null)
        {
            _entity.OnPropertyUpdated -= OnPropertyUpdated;
            _entity = null;   
        }
    }

    private void OnPropertyUpdated()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        _idInput.SetValue(_entity.Id.ToString());
        _nameInput.SetValue(_entity.Name);
        _positionInput.SetValue(_entity.Position);
        _angleInput.SetValue(_entity.Angle);
        _depthInput.SetValue(_entity.ZDepth);

        foreach (BaseEntityMenu menu in _entityMenus)
        {
            menu.UpdateUI();
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

    public void OnDepthSubmit(float value)
    {
        _entity.ZDepth = (int)value;
    }

    public void OpenGraph()
    {
        SceneEntity selected = ObjectManager.Instance.SelectedObject;
        ScriptGraph.Instance.TogglePanel(selected.Script);
    }

    public void ChooseTexture()
    {
        TextureController.Instance.OpenMenu(_entity);     
    }
}