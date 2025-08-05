using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ListElementInputField : MonoBehaviour
{
    public UnityAction<ListElementInputField, string> OnEndEdit;
    public UnityAction<ListElementInputField> OnRemove;

    public RectTransform Rect;
    public TMP_InputField InputField;
    public Button RemoveButton;

    private void Awake()
    {
        InputField.onEndEdit.AddListener(EndEdit);
        RemoveButton.onClick.AddListener(Remove);
    }

    private void Remove()
    {
        OnRemove?.Invoke(this);
    }

    private void EndEdit(string value)
    {
        OnEndEdit?.Invoke(this, value);
    }

    public void SetValue(object value)
    {
        InputField.SetTextWithoutNotify(value.ToString());
    }
}