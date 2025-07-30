using UnityEngine;
using UnityEngine.InputSystem;

public class SceneInteraction : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private ITool _tool;
    private ITool[] _tools = new ITool[]
    {
        new MoveTool(),
        new RotateTool(),
        new BoxTool(),
        new CircleTool()
    };

    private void Start()
    {
        _tool = _tools[0];
    }

    private void Update()
    {
        HandleSelection();

        _tool.OnUpdate(MouseWorldPositon());
    }

    private void HandleSelection()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            Vector3 worldPoint = _camera.ScreenToWorldPoint(mousePosition);

            ObjectManager.Instance.Select(worldPoint);
        }
    }

    private Vector3 MouseWorldPositon()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldPoint = _camera.ScreenToWorldPoint(mousePosition);
        worldPoint.z = 0;
        return worldPoint;
    }

    public void SwitchTool(int index)
    {
        _tool = _tools[index];
    }
}