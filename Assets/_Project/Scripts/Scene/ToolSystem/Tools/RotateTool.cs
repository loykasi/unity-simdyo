using UnityEngine;
using UnityEngine.InputSystem;

public class RotateTool : ITool
{
    public ToolType Type => ToolType.Rotate;

    private bool _onRotation = false;
    private Vector3 _fromDirection;
    private float _startAngle;

    public void Disable()
    {
        
    }

    public void Enable()
    {
        
    }

    public void OnUpdate(Vector3 mousePosition)
    {   
        if (Mouse.current.leftButton.wasPressedThisFrame && !ScreenInteractionUtils.IsOverUI())
        {
            var selected = ObjectManager.Instance.SelectedObject;
            if (selected == null)
            {
                return;
            }
            _fromDirection = mousePosition - selected.transform.position;
            _startAngle = selected.transform.eulerAngles.z;

            _onRotation = true;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _onRotation = false;
        }

        if (_onRotation)
        {
            var selected = ObjectManager.Instance.SelectedObject;
            Vector3 toDirection = mousePosition - selected.transform.position;

            float angle = Vector3.SignedAngle(_fromDirection, toDirection, Vector3.forward);
            selected.transform.rotation = Quaternion.Euler
                                            (
                                                0f,
                                                0f,
                                                _startAngle + angle
                                            );

            Debug.Log(angle);
            Debug.DrawRay(selected.transform.position, _fromDirection, Color.red);
            Debug.DrawRay(selected.transform.position, toDirection, Color.blue);
        }
    }
}