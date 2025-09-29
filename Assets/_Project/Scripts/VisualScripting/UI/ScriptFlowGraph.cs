using UnityEngine;

namespace Loykas.Scripting
{
    public class ScriptFlowGraph : MonoBehaviour
    {
        public ScriptFlow Flow { get; set; }
        public SceneEntity Entity => Flow.Entity;

        [SerializeField] private VariableBoard _variableBoard;
        [SerializeField] private NodeBoard _nodeBoard;
        [SerializeField] private FunctionBoard _functionBoard;
        [SerializeField] private SizeBar _sidebar;

        private void Awake()
        {
            _variableBoard.FlowGraph = this;
            _nodeBoard.FlowGraph = this;
            _functionBoard.FlowGraph = this;
        }

        public void Open(ScriptFlow flow)
        {
            Flow = flow;
            _variableBoard.Init();
            _nodeBoard.Init();
        }

        public void Close()
        {
            _nodeBoard.Close();
        }

        public void RebuildSideBarUI()
        {
            _sidebar.RebuildUI();
        }
    }
}