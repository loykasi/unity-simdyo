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
            Completed = OutputTrigger(nameof(Completed)).ShowLabel();
            LoopBody = OutputTrigger(nameof(LoopBody)).ShowLabel();

            List = InputValue(nameof(List), ScriptDataType.List(DataType.Any));
            Element = OutputValue(nameof(Element), ScriptDataType.Single(DataType.Entity), GetElement);
            Index = OutputValue(nameof(Index), ScriptDataType.Single(DataType.Number), GetIndex);
        }

        public override ScriptNode Create()
        {
            return new ForEachNode();
        }

        private object GetElement()
        {
            return _list[_index];
        }

        private object GetIndex()
        {
            return _index;
        }

        private OutputTrigger Loop(NodeTask task)
        {
            _list = (IList)List.GetValue();
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