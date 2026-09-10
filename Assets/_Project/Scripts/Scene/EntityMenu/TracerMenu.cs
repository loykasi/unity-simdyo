using UnityEngine;

public class TracerMenu : BaseEntityMenu
{
    [Header("Properties")]
    [SerializeField] private EntityMenuInputBase[] _inputs;

    public override void Setup()
    {
        
    }

    public override void UpdateUI()
    {
        EntityGroup group = ObjectManager.Instance.SelectionGroup;

        if (group.TracerCount > 0)
        {
            _menuGameObject.SetActive(true);
            foreach (var input in _inputs)
            {
                input.UpdateValue();
            }
        }
        else
        {
            _menuGameObject.SetActive(false);
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
}