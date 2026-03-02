using System;
using System.Collections.Generic;
using UnityEngine;

public class UIToolSelection : MonoBehaviour
{
    [SerializeField] private RectTransform _selectionBorder;
    [SerializeField] private UIToolButton[] _toolElements;

    private Dictionary<ToolType, UIToolButton> _toolElementTable = new();
    // private Vector2 _selectionBorderOffset = new(- 10f, 10f);

    private void Awake()
    {
        InitToolButton();
        CreateToolElementTable();
    }

    private void OnEnable()
    {
        ToolManagement.Instance.OnToolChanged += OnToolChanged;
    }

    private void InitToolButton()
    {
        foreach (UIToolButton button in _toolElements)
        {
            button.Init(this);
        }
    }

    private void CreateToolElementTable()
    {
        foreach (UIToolButton item in _toolElements)
        {
            if (!_toolElementTable.ContainsKey(item.Type))
            {
                _toolElementTable.Add(item.Type, item);
            }
        }
    }

    private void OnToolChanged()
    {
        if (!ToolManagement.Instance.HasTool)
        {
            return;
        }

        ToolType type = ToolManagement.Instance.CurrentTool;
        SetSelectionBorder(type);
    }

    public void SelectTool(ToolType type)
    {
        ToolManagement.Instance.SwitchTool(type);
    }

    public void SetSelectionBorder(ToolType type)
    {
        if (_toolElementTable.TryGetValue(type, out UIToolButton element))
        {
            _selectionBorder.gameObject.SetActive(true);
            _selectionBorder.position = element.transform.position;
            // _selectionBorder.anchoredPosition += _selectionBorderOffset;
        }
        else
        {
            _selectionBorder.gameObject.SetActive(false);
        }
    }
}