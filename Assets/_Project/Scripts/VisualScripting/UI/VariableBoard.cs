using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class VariableBoard : MonoBehaviour
{
    [SerializeField] private VariableBoardItem _itemPrefab;
    [SerializeField] private Transform _contentHolder;

    [Space]
    [SerializeField] private TMP_InputField _nameInputField;

    public void AddVariable()
    {
        VisualScripting vs = NodeBoard.Instance.TargetVisualScripting;
        if (vs != null)
        {
            string name = _nameInputField.text;
            if (vs.AddVariable(name))
            {
                VariableBoardItem item = Instantiate(_itemPrefab, _contentHolder);
                item.Init(name, this);
            }
        }
    }

    public void UpdateVariable(string name, DataType type, object value)
    {
        VisualScripting vs = NodeBoard.Instance.TargetVisualScripting;
        vs.UpdateVariable(name, type, value);
    }

    // public List<RaycastResult> results = new();
    // private void Update()
    // {
    //     if (Mouse.current.leftButton.wasPressedThisFrame)
    //     {
    //         var data = new PointerEventData(EventSystem.current)
    //         {
    //             position = Mouse.current.position.ReadValue()
    //         };
    //         EventSystem.current.RaycastAll(data, results);

    //         if (results.Count == 0)
    //         {
    //             return;
    //         }

    //         Debug.Log($"{results[0]}");
    //     }

    //     if (Mouse.current.leftButton.wasReleasedThisFrame)
    //     {

    //     }
    // }
}