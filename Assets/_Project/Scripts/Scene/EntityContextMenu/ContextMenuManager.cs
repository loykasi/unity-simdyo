using System.Collections.Generic;
using UnityEngine;

public class ContextMenuManager : Singleton<ContextMenuManager>
{
    private readonly Dictionary<ContextMenuActions, ContextMenuAction> _actionTable = new()
    {
        { ContextMenuActions.Clone, new CloneContextMenuAction() },
        { ContextMenuActions.Delete, new DeleteContextMenuAction() },
        { ContextMenuActions.Intersect, new IntersectContextMenuAction() },
        { ContextMenuActions.Subtract, new SubtractContextMenuAction() },
        { ContextMenuActions.MoveToBack, new MoveToBackContextMenuAction() },
        { ContextMenuActions.MoveToFront, new MoveToFrontContextMenuAction() },
        { ContextMenuActions.ResizeByTexture, new ResizeByTextureContextMenuAction() },
    };

    public void Open(Vector3 screenPosition)
    {
        bool isOverEntity = ObjectManager.Instance.TryGetSceneEntity(CameraManager.Instance.EditorCamera, screenPosition, out SceneEntity hovered);

        if (!isOverEntity)
        {
            return;
        }

        if (!ObjectManager.Instance.SelectionGroup.Contains(hovered))
        {
            return;
        }

        UIManager.Instance.EntityContextMenu.Open();
    }

    public void Execute(ContextMenuActions type)
    {
        if (_actionTable.TryGetValue(type, out ContextMenuAction action))
        {
            action.Execute(ObjectManager.Instance.SelectionGroup);
            KDebug.Log($"[ContextMenu] Execute {type}");
        }
        UIManager.Instance.EntityContextMenu.Close();
    }

    public ContextMenuAction GetAction(ContextMenuActions type)
    {
        return _actionTable.TryGetValue(type, out ContextMenuAction action) ? action : null;
    }
}
