using System;
using UnityEngine;
using UnityEngine.UI;

namespace Loykas.Scripting
{
    public class PortVisual : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UITypeColorData _typeColorData;

        [Header("Handle")]
        [SerializeField] protected RectTransform _portHandle;
        [SerializeField] private Image _handleImage;
        
        [SerializeField] private Sprite _triggerHandleSprite;
        [SerializeField] private Sprite _triggerHandleConnectedSprite;

        [SerializeField] private Sprite _valueHandleSprite;
        [SerializeField] private Sprite _valueHandleConnectedSprite;

        [SerializeField] private Sprite _listHandleSprite;
        [SerializeField] private Sprite _listHandleConnectedSprite;

        private NodePortType _type = NodePortType.Trigger;
        private ScriptDataType _dataType;

        public void SetPortType(NodePortType type)
        {
            _type = type;
        }

        public void SetType(ScriptDataType type)
        {
            _dataType = type;
            var display = _typeColorData.Get(type.Type);

            _handleImage.sprite = _dataType.Kind == DataKind.List ? _listHandleSprite : _valueHandleSprite;
            _handleImage.color = display.Color;
        }

        public void SetConnectionStatus(bool value)
        {
            if (_type == NodePortType.Trigger)
            {
                SetTriggerConnectionStatus(value);
            }
            else if (_type == NodePortType.Value)
            {
                SetValueConnectionStatus(value);
            }
        }

        private void SetTriggerConnectionStatus(bool value)
        {
            if (value)
            {
                _handleImage.sprite = _triggerHandleConnectedSprite;
            }
            else
            {
                _handleImage.sprite = _triggerHandleSprite;
            }
        }

        private void SetValueConnectionStatus(bool value)
        {
            if (value)
            {
                _handleImage.sprite = _dataType.Kind == DataKind.List ? _listHandleConnectedSprite : _valueHandleConnectedSprite;
            }
            else
            {
                _handleImage.sprite = _dataType.Kind == DataKind.List ? _listHandleSprite : _valueHandleSprite;
            }
        }
    }
}