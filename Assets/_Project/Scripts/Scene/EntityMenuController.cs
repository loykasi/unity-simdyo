using System;
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
        _menu.gameObject.SetActive(false);
    }

    private void OnObjectSelected(SceneEntity entity)
    {
        if (entity != null)
        {
            _menu.gameObject.SetActive(true);
            _entity = entity;
            _menu.Init(_entity);
        }
    }

    public void ToggleGravity(bool value)
    {
        _entity.ToggleGravity(value);
    }

    public void ToggleCollider(bool value)
    {
        _entity.ToggleCollider(value);
    }
}