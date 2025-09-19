using System;
using UnityEngine;

public class UIValueOutputPort: UINodePort
{
    public override NodePortEdge Edge => NodePortEdge.Right;

    [SerializeField] private PortVisual _portVisual;
    private OutputValue _outputValue;

    private readonly float _handleSize = 70f;
    private readonly float _height = 30f;

    public override void Init()
    {
        base.Init();

        if (Port is not OutputValue)
        {
            Debug.LogError("Wrong port assignment.", this);
            return;
        }
        _outputValue = (OutputValue)Port;

        _portVisual.SetType(_outputValue.Type);

        UpdateSize();
    }

    private void UpdateSize()
    {
        Vector2 size = _label.GetPreferredValues();
        _label.rectTransform.sizeDelta = new Vector2
        (
            size.x,
            _label.rectTransform.sizeDelta.y
        );

        Rect.sizeDelta = new Vector2
        (
            _handleSize + size.x,
            _height
        );
    }
}