using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{
    class GetListItemNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.List;

        public InputValue ListInput;
        public InputValue Index;
        public OutputValue Value;

        public override ScriptNode Create()
        {
            return new GetListItemNode();
        }

        public GetListItemNode()
        {
            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any))
                        .UseGlobalLocalized();
            
            Index = InputValue(nameof(Index), ScriptDataType.Single(DataType.Number))
                    .UseInput()
                    .UseGlobalLocalized();

            Value = OutputValue(nameof(Value), ScriptDataType.Single(DataType.Any), Get)
                    .UseGlobalLocalized();

            ListInput.OnConnected += OnListInputConnected;
        }

        public override void Init()
        {
            UpdateNode();
        }

        private object Get()
        {
            IList list = (IList)ListInput.GetValue();

            object value = Index.GetValue();
            int index;
            if (value is float floatValue)
            {
                index = (int)floatValue;
            }
            else
            {
                index = (int)value;
            }
            
            return list[index];
        }

        private void OnListInputConnected()
        {
            UpdateNode();
        }

        public override void UpdateNode()
        {
            if (ListInput.Source != null)
            {
                DataType type = ListInput.Source.Type.Type;
                Value.SetType(ScriptDataType.Single(type));
            }
            else
            {
                Value.SetType(ScriptDataType.Single(DataType.Any));
            }

            OnNodeUpdated?.Invoke();
        }
    }
}