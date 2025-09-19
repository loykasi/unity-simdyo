using TMPro;
using UnityEngine;

public class FunctionBoardItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _label;

    public void Init(string label)
    {
        _label.text = label;
    }
}