using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListInputField : VariableInput
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private RectTransform _addButton;
    [SerializeField] private RectTransform _container;
    [SerializeField] private ListElementInputField _inputElementPrefab;
    private List<ListElementInputField> _inputElements = new();
    private List<string> _input = new();
    private int _inputCount = 0;

    public void Add()
    {
        AddInputField();
        VariableItem.InsertListItem("");
    }

    public void AddInputField(object value = null)
    {
        var element = Instantiate(_inputElementPrefab, _container);
        float height = element.Rect.sizeDelta.y;

        element.Rect.localPosition = new Vector3(0f, -height * _inputCount, 0f);
        _inputCount++;

        _container.sizeDelta = new Vector2
        (
            _container.sizeDelta.x,
            height * _inputCount
        );

        _addButton.localPosition = new Vector3(0f, -_container.sizeDelta.y, 0f);
        _rect.sizeDelta = new Vector2
        (
            _rect.sizeDelta.x,
            _container.sizeDelta.y + _addButton.sizeDelta.y
        );

        _inputElements.Add(element);

        element.OnEndEdit += OnEndEdit;
        element.OnRemove += OnRemove;

        if (value != null)
        {
            element.SetValue(value);
        }
    }

    public void Remove(int index)
    {
        var element = _inputElements[index];

        float height = element.Rect.sizeDelta.y;

        Destroy(element.gameObject);
        _inputCount--;

        _container.sizeDelta = new Vector2
        (
            _container.sizeDelta.x,
            height * _inputCount
        );

        _addButton.localPosition = new Vector3(0f, -_container.sizeDelta.y, 0f);
        _rect.sizeDelta = new Vector2
        (
            _rect.sizeDelta.x,
            _container.sizeDelta.y + _addButton.sizeDelta.y
        );

        for (int i = index + 1; i < _inputElements.Count; i++)
        {
            _inputElements[i].Rect.localPosition += new Vector3(0f, height, 0f);
        }

        _inputElements.RemoveAt(index);
        VariableItem.RemoveListItem(index);
    }

    private void OnEndEdit(ListElementInputField element, string value)
    {
        Debug.Log($"update index {IndexOfElement(element)} = {value}");
        VariableItem.UpdateListItem(IndexOfElement(element), value);
    }

    private void OnRemove(ListElementInputField element)
    {
        Debug.Log($"Remove {IndexOfElement(element)}");
        Remove(IndexOfElement(element));
    }

    private int IndexOfElement(ListElementInputField element)
    {
        return _inputElements.IndexOf(element);
    }

    public override void Disable()
    {
        gameObject.SetActive(false);
    }

    public override void Enable()
    {
        gameObject.SetActive(true);
    }

    public override object GetValue()
    {
        throw new System.NotImplementedException();
    }

    public override void SetValue(object value)
    {
        IList list = (IList)value;
        for (int i = 0; i < list.Count; i++)
        {
            AddInputField(list[i]);
        }
    }
}
