using UnityEngine;
using UnityEngine.InputSystem;

public class RotateTool : ITool
{
    private bool _onRotation = false;
    private Vector3 _fromDirection;
    private float _startAngle;

    public void OnUpdate(Vector3 mousePosition)
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            GameObject selected = ObjectManager.Instance.SelectedObject;
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
            GameObject selected = ObjectManager.Instance.SelectedObject;
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