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

        public ForNode(string title) : base(title)
        {
            Enter = InputTrigger(nameof(Enter), Loop);
            Completed = OutputTrigger(nameof(Completed));
            LoopBody = OutputTrigger(nameof(LoopBody));

            FirstIndex = InputValue(nameof(FirstIndex), ScriptDataType.Single(DataType.Number));
            LastIndex = InputValue(nameof(LastIndex), ScriptDataType.Single(DataType.Number));
            Step = InputValue(nameof(Step), ScriptDataType.Single(DataType.Number));
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
            int loop = vs.StartLoop();
            // Debug.Log($"Start loop {loop}");

            int firstIndex = (int)(double)FirstIndex.GetValue(vs);
            int lastIndex = (int)(double)LastIndex.GetValue(vs);
            int step = (int)(double)Step.GetValue(vs);
            bool isAscending = firstIndex <= lastIndex;

            int index = firstIndex;

            while (vs.IsLoopNotBroken(loop) && CanMoveNext(index, lastIndex, isAscending))
            {
                // Debug.Log($"{loop} | loop {index}");

                _index = index;
                LoopBody.Invoke(vs);
                index += step;
            }

            vs.ExitLoop(loop);

            return Completed;
        }

        private bool CanMoveNext(int index, int lastIndex, bool isAscending)
        {
            return isAscending ? (index <= lastIndex) : (index >= lastIndex);
        }
    }
}