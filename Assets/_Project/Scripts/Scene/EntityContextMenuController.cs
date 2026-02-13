using UnityEngine;

public class EntityContextMenuController : Singleton<EntityContextMenuController>
{
    [SerializeField] private EntityContextMenu _contextMenu;

    public void Open()
    {
        if (ObjectManager.Instance.SelectedObject != null)
        {
            _contextMenu.Open();
        }
    }
}