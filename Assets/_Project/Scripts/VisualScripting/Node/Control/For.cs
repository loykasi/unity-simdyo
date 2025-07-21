using UnityEngine;

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
    public InputTrigger InputTrigger;
    public OutputTrigger Completed;
    public OutputTrigger LoopBody;

    public ValueInput FirstIndex;
    public ValueInput LastIndex;
    public ValueInput Step;
    public ValueOutput Index;

    private int _index;

    public ForNode(string title) : base(title)
    {
        InputTrigger = CreateInputTrigger(Loop);
        Completed = CreateOutputTrigger();
        LoopBody = CreateOutputTrigger();

        FirstIndex = ValueInput(Variable.Number, true);
        LastIndex = ValueInput(Variable.Number, true);
        Step = ValueInput(Variable.Number, true);
        Index = ValueOutput(() =>
        {
            return _index;
        });
    }

    private OutputTrigger Loop(VisualScripting vs)
    {
        int loop = vs.StartLoop();
        // Debug.Log($"Start loop {loop}");

        int firstIndex = (int)(double)FirstIndex.GetValue();
        int lastIndex = (int)(double)LastIndex.GetValue();
        int step = (int)(double)Step.GetValue();
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