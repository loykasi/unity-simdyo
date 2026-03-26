using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{
    class ForEachNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Control;

        public InputTrigger Enter;
        public OutputTrigger Completed;
        public OutputTrigger LoopBody;

        public InputValue List;
        public OutputValue Element;
        public OutputValue Index;

        private IList _list;
        private int _index;
        private bool _firstRun = true;

        public ForEachNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Loop);

            Completed = CreateOutputTrigger
            (
                nameof(Completed),
                PortSettings.Default
            );

            LoopBody = CreateOutputTrigger
            (
                nameof(LoopBody),
                PortSettings.Default
            );

            List = CreateInputValue
            (
                nameof(List),
                ScriptDataType.List(DataType.Any),
                PortSettings.Default
            );

            Element = CreateOutputValue
            (
                nameof(Element),
                GetElement,
                ScriptDataType.Single(DataType.Entity),
                PortSettings.Default
            );

            Index = CreateOutputValue
            (
                nameof(Index),
                GetIndex,
                ScriptDataType.Single(DataType.Number),
                PortSettings.Default
            );
        }

        public override ScriptNode Create()
        {
            return new ForEachNode();
        }

        private ValueTransfer GetElement()
        {
            return ValueTransfer.FromValue(_list[_index]);
        }

        private ValueTransfer GetIndex()
        {
            return ValueTransfer.CreateNumber(_index);
        }

        private OutputTrigger Loop(NodeTask task)
        {
            _list = (IList)List.GetValue().RefValue;
            int firstIndex = 0;
            int lastIndex = _list.Count;
            int step = 1;

            if (task.ShouldBreak)
            {
                _firstRun = true;
                task.ShouldBreak = false;
                return Completed;
            }

            if (_firstRun)
            {
                // Debug.Log($"first run: step {step}");
                task.EnterLoop(Enter);
                _index = firstIndex;

                _firstRun = false;
                return LoopBody;
            }

            _index += step;

            if (_index > lastIndex)
            {
                _firstRun = true;
                _list = null;
                return Completed;
            }
            
            task.EnterLoop(Enter);
            return LoopBody;
        }
    }
}