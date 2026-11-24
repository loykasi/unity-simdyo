using UnityEngine;

public class UISceneEditor : MonoBehaviour
{
    public void OnActionPlayClicked()
    {
        EngineManager.Instance.RequestPlay();
    }

    public void OnActionGridClicked(bool value)
    {
        GridController.Instance.ToggleGrid(value);
    }
}