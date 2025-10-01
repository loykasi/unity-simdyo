using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "For", menuName = "Scriptable Objects/Visual Scripting/Node/For")]
    public class For : ScriptNodeData
    {
        public override ScriptNode Create()
        {
            return new ForNode(Title);
        }
    }

    class ForNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Completed;
        public OutputTrigger LoopBody;

        public InputValue FirstIndex;
        public InputValue LastIndex;
        public InputValue Step;
        public OutputValue Index;

        private int _index;
        private bool _firstRun;

        public ForNode(string title) : base(title)
        {
            Enter = InputTrigger(nameof(Enter), Loop);
            Completed = OutputTrigger(nameof(Completed));
            LoopBody = OutputTrigger(nameof(LoopBody));

            FirstIndex = InputValue(nameof(FirstIndex), ScriptDataType.Single(DataType.Number)).UseInput();
            LastIndex = InputValue(nameof(LastIndex), ScriptDataType.Single(DataType.Number)).UseInput();
            Step = InputValue(nameof(Step), ScriptDataType.Single(DataType.Number)).UseInput();

            Index = OutputValue(
                nameof(Index),
                (vs) =>
                {
                    return _index;
                }
            );
        }

        private OutputTrigger Loop(ScriptFlow vs)
        {
            int firstIndex = (int)(float)FirstIndex.GetValue(vs);
            int lastIndex = (int)(float)LastIndex.GetValue(vs);
            int step = (int)(float)Step.GetValue(vs);

            NodeTask task = vs.GetNodeTask(Enter);

            if (_firstRun)
            {
                task.EnterLoop(Enter);
                _index = firstIndex;

                _firstRun = false;
                return LoopBody;
            }

            _index += step;

            if (_index > lastIndex)
            {
                _firstRun = true;
                return Completed;
            }
            
            task.EnterLoop(Enter);
            return LoopBody;
        }

        // private OutputTrigger Loop(ScriptFlow vs)
        // {
        //     int firstIndex = (int)(float)FirstIndex.GetValue(vs);
        //     int lastIndex = (int)(float)LastIndex.GetValue(vs);
        //     int step = (int)(float)Step.GetValue(vs);

        //     int loop = vs.StartLoop();

        //     bool isAscending = firstIndex <= lastIndex;


        //     // vs.Invoke(LoopBody, OnBodyFinish);

        //     int index = firstIndex;

        //     while (vs.IsLoopNotBroken(loop) && CanMoveNext(index, lastIndex, isAscending))
        //     {
        //         _index = index;
        //         LoopBody.Invoke(vs);
        //         index += step;
        //     }

        //     vs.ExitLoop(loop);

        //     return Completed;
        // }

        private bool CanMoveNext(int index, int lastIndex, bool isAscending)
        {
            return isAscending ? (index <= lastIndex) : (index >= lastIndex);
        }
    }
}