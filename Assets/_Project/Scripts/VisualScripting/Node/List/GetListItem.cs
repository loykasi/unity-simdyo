using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{
    class GetListItemNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.List;

        public InputValue ListInput;
        public InputValue Index;
        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new GetListItemNode();
        }

        public GetListItemNode()
        {
            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any));
            Index = InputValue(nameof(Index), ScriptDataType.Single(DataType.Number)).UseInput();
            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Any), Get);

            ListInput.OnConnected += OnListInputConnected;
        }

        private object Get()
        {
            IList list = (IList)ListInput.GetValue();
            int index = (int)(float)Index.GetValue();
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
                Output.SetType(ScriptDataType.Single(type));
            }
            else
            {
                Output.SetType(ScriptDataType.Single(DataType.Any));
            }

            OnNodeUpdated?.Invoke();
        }
    }
}