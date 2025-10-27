using UnityEngine;
using UnityEngine.InputSystem;

public class RotateTool : PanTool
{
    public override ToolType Type => ToolType.Rotate;

    private bool _onRotation = false;
    private Vector3 _fromDirection;
    private float _startAngle;
    private SceneEntity _entity;
    private Quaternion _startQuaternion;

    public override void OnUpdate()
    {
        Zoom();
        HandlePanRightMouse();
        HandleRotate();
    }

    private void HandleRotate()
    {
        Vector3 mousePosition = GetMouseWorldPositon();
        if (Mouse.current.leftButton.wasPressedThisFrame && !ScreenInteractionUtils.IsOverUI())
        {
            _entity = ObjectManager.Instance.SelectedObject;
            if (_entity == null)
            {
                return;
            }
            _fromDirection = mousePosition - _entity.transform.position;
            _startAngle = _entity.transform.eulerAngles.z;

            _onRotation = true;

            RotateController.Instance.EnableVisualization(_entity.transform.position, mousePosition);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _onRotation = false;
            RotateController.Instance.DisableVisualization();
        }

        if (_onRotation)
        {
            Vector3 toDirection = mousePosition - _entity.transform.position;

            float angle = Vector3.SignedAngle(_fromDirection, toDirection, Vector3.forward);
            angle = _startAngle + angle;

            if (RotateController.Instance.IsSnapping(mousePosition, _entity.transform.position))
            {
                angle = Mathf.Round(angle / 15f) * 15f;
            }
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            _entity.Rotation = rotation;

            RotateController.Instance.UpdateUI(_entity.transform.position, mousePosition);
            
            Physics2D.SyncTransforms();

            // Debug.Log(angle);
            Debug.DrawRay(_entity.transform.position, _fromDirection, Color.red);
            Debug.DrawRay(_entity.transform.position, toDirection, Color.blue);
        }
    }
}