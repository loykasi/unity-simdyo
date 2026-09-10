using UnityEngine;

public class EntityMenu : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] private EntityMenuInputBase[] _inputs;
    [SerializeField] private GameObject _actions;

    [Header("Type")]
    [SerializeField] private BaseEntityMenu[] _entityMenus;

    public void Setup()
    {
        foreach (BaseEntityMenu menu in _entityMenus)
        {
            menu.Setup();
        }
    }

    public void Open()
    {
        UpdateUI();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        foreach (BaseEntityMenu menu in _entityMenus)
        {
            menu.Close();
        }
        gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        _actions.SetActive(ObjectManager.Instance.SelectionGroup.Count == 1);

        foreach (EntityMenuInputBase input in _inputs)
        {
            input.UpdateValue();
        }

        foreach (BaseEntityMenu menu in _entityMenus)
        {
            menu.UpdateUI();
        }
    }

    public void OpenGraph()
    {
        if (ObjectManager.Instance.SelectionGroup.Count > 1)
        {
            KDebug.Log("[Entity Menu] No multiple objects supported");
        }
        
        SceneEntity entity = ObjectManager.Instance.SelectionGroup.Entities[0];
        ScriptGraph.Instance.TogglePanel(entity.Script);
    }

    public void ChooseTexture()
    {
        if (ObjectManager.Instance.SelectionGroup.Count > 1)
        {
            KDebug.Log("[Entity Menu] No multiple objects supported");
        }

        SceneEntity entity = ObjectManager.Instance.SelectionGroup.Entities[0];
        TextureController.Instance.OpenMenu(entity);
    }
}