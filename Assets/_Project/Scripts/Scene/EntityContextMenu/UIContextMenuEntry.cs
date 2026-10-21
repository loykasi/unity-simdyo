using UnityEngine;

public class UIContextMenuEntry : MonoBehaviour
{
    public ContextMenuActions Type;

    public void Execute()
    {
        ContextMenuManager.Instance.Execute(Type);
    }
}
