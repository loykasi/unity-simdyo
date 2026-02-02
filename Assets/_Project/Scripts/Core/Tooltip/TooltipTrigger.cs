using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private LocalizedString _localizedStringContent;
    private string _contentText;

    private void OnEnable()
    {
        _localizedStringContent.StringChanged += UpdateString;
    }

    private void OnDisable()
    {
        _localizedStringContent.StringChanged -= UpdateString;
    }

    void UpdateString(string s)
    {
        _contentText = s;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Instance.Show(_contentText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Instance.Hide();
    }
}