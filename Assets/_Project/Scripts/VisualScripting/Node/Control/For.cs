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
        InputTrigger = CreateInputTrigger(() =>
        {
            int firstIndex = (int) FirstIndex.GetValue();
            int lastIndex = (int) LastIndex.GetValue();
            int step = (int) Step.GetValue();
            for (int i = firstIndex; i <= lastIndex; i += step)
            {
                _index = i;
                Debug.Log($"loop {i}");
                LoopBody.Invoke();
            }
            return Completed;
        });
        Completed = CreateOutputTrigger();
        LoopBody = CreateOutputTrigger();

        FirstIndex = ValueInput();
        LastIndex = ValueInput();
        Step = ValueInput();
        Index = ValueOutput(() =>
        {
            return _index;
        });
    }
}