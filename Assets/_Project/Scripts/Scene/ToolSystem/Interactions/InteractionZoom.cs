using System;
using UnityEngine;

public class InteractionZoom
{
    private float TargetHeight
    {
        get => EngineManager.Instance.EditorCameraHeight;
        set => EngineManager.Instance.EditorCameraHeight = value;
    }

    private Action<float> _scrollEvent;

    public InteractionZoom()
    {
        _scrollEvent = OnScroll;
    }

    public void Enable()
    {
        InputManager.Instance.ScrollEvent += _scrollEvent;
    }

    public void Disable()
    {
        InputManager.Instance.ScrollEvent -= _scrollEvent;
    }

    private void OnScroll(float value)
    {
        if (ScreenInteractionUtils.IsOverUI())
        {
            return;
        }

        float scroll = value;
        if (scroll != 0)
        {
            Vector2 limit = EngineManager.Instance.ZoomHeighLimit;

            float zoomValue = EngineManager.Instance.ZoomSpeed;
            float zoomFactor = Mathf.Sign(scroll) > 0 ? 1f / zoomValue : zoomValue;
            TargetHeight = Mathf.Clamp(TargetHeight * zoomFactor, limit.x, limit.y);
        }
    }

    private float ExponentialDecay(float a, float b, float decay, float dt)
    {
        return b + (a-b) * Mathf.Exp(-decay * dt);
    }

    public void Update()
    {
        Camera camera = EngineManager.Instance.EditorCamera;
        Vector3 mousePosition = Utils.ToWorldPositon(InputManager.Instance.MousePosition);

        camera.orthographicSize = ExponentialDecay(camera.orthographicSize, TargetHeight, EngineManager.Instance.SmoothFactor, Time.unscaledDeltaTime);

        Vector3 offset = mousePosition - Utils.ToWorldPositon(InputManager.Instance.MousePosition);
        camera.transform.position += offset;
    }
}