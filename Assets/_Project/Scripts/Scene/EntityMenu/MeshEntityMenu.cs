using System;
using UnityEngine;

public class MeshEntityMenu : BaseEntityMenu
{
    [Header("Properties")]
    [SerializeField] private EntityMenuInputBase[] _inputs;

    [Header("Box")]
    [SerializeField] private GameObject _boxMenu;
    [SerializeField] private EntityMenuInputBase[] _boxInputs;
    [SerializeField] private GameObject _boxActions;

    [Header("Circle")]
    [SerializeField] private GameObject _circleMenu;
    [SerializeField] private EntityMenuInputBase[] _circleInputs;

    [Header("Collision layers")]
    [SerializeField] private EntityMenuInput_CollisionLayer _collisionLayerInput;

    public override void Setup()
    {
        _collisionLayerInput.Setup();
    }
    
    public override void UpdateUI()
    {
        EntityGroup group = ObjectManager.Instance.SelectionGroup;

        bool isEnabled = group.MeshEntityCount > 0;
        if (!isEnabled) return;

        _menuGameObject.SetActive(isEnabled);
        _boxMenu.SetActive(group.BoxCount > 0);
        _circleMenu.SetActive(group.CircleCount > 0);

        foreach (EntityMenuInputBase input in _inputs)
        {
            input.UpdateValue();
        }

        _collisionLayerInput.UpdateValue();

        if (group.BoxCount > 0)
        {
            foreach (EntityMenuInputBase input in _boxInputs)
            {
                input.UpdateValue();
            }
            _boxActions.SetActive(group.Count == 1);
        }

        if (group.CircleCount > 0)
        {
            foreach (EntityMenuInputBase input in _circleInputs)
            {
                input.UpdateValue();
            }   
        }
    }
    
    public override void Show()
    {
        _menuGameObject.SetActive(true);
    }

    public override void Close()
    {
        _menuGameObject.SetActive(false);
    }

    public void OpenTextEditor()
    {
        if (ObjectManager.Instance.SelectionGroup.Count > 1)
        {
            KDebug.Log("[Entity Menu] No multiple objects supported");
        }

        BoxEntity entity = (BoxEntity)ObjectManager.Instance.SelectionGroup.Entities[0];
        TextBoxMenu.Instance.Open(entity.TextBox);
    }
}