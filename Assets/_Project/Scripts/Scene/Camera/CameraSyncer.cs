using UnityEngine;

public class CameraSyncer : MonoBehaviour
{
    [SerializeField] private Camera _targetCamera;
    [SerializeField] private Camera _currentCamera;

    private void LateUpdate()
    {
        if (_targetCamera.orthographicSize != _currentCamera.orthographicSize)
        {
            _currentCamera.orthographicSize = _targetCamera.orthographicSize;
        }
    }
}