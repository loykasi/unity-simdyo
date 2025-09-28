using UnityEngine;

public class UIOutputTriggerPort: UINodePort
{
    public override NodePortEdge Edge => NodePortEdge.Right;
    private readonly float _handleSize = 20f;
    private readonly float _height = 30f;

    public override void Init()
    {
        base.Init();

        UpdateSize();
    }

    private void UpdateSize()
    {
        UpdateLabel();

        Rect.sizeDelta = new Vector2
        (
            _handleSize + _label.rectTransform.sizeDelta.x,
            _height
        );
    }
}