using UnityEngine;

public class InteractionPan
{
    private Vector3 _origin;
    private bool _isInitialized;
    private bool _isEnabled;

    public void Start()
    {
        if (!_isEnabled)
        {
            _isEnabled = true;
            _isInitialized = false;   
        }
    }

    public void Stop()
    {
        _isEnabled = false;
    }

    public void Pan(Vector2 value)
    {
        if (_isEnabled)
        {
            if (!_isInitialized)
            {
                _origin = Utils.ToWorldPositon(value);
                _isInitialized = true;
            }
            Vector3 mousePosition = Utils.ToWorldPositon(value);
            Vector3 delta = _origin - mousePosition;
            EngineManager.Instance.EditorCamera.transform.position += delta;
        }
    }
}