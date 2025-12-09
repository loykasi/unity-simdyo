using System;
using UnityEngine;

namespace Loykas.Scripting
{
    public class UIValueOutputPort : UINodePort
    {
        public override NodePortEdge Edge => NodePortEdge.Right;
        public override NodePortType Type => NodePortType.Value;

        private OutputValue _outputValue;

        private readonly float _handleSize = 20f;
        private readonly float _height = 30f;

        public override void Init()
        {
            base.Init();

            if (Port is not OutputValue)
            {
                Debug.LogError($"Wrong port assignment. {Port.GetType()}", this);
                return;
            }
            _outputValue = (OutputValue)Port;

            _portVisual.SetType(_outputValue.Type);

            UpdateSize();
        }

        public override void UpdateUI()
        {
            base.UpdateUI();
            _portVisual.SetType(_outputValue.Type);
            UpdateSize();
        }

        private void UpdateSize()
        {
            UpdateLabel();

            float width = _handleSize;

            if (_outputValue.ShouldShowLabel)
            {
                width += _label.rectTransform.sizeDelta.x;
            }

            Rect.sizeDelta = new Vector2(width, _height);
        }
    }
}