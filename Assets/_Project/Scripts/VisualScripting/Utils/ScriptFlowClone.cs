namespace Loykas.Scripting
{
    public static class ScriptFlowClone
    {
        public static void CloneScript(ScriptFlow a, ScriptFlow b)
        {
            foreach (Variable targetVariable in a.VariableList)
            {
                Variable variable = ScriptPool.Instance.Variable.Get();
                variable.Init(targetVariable);
                b.AddVariable(variable);
            }

            foreach (ScriptFunction targetFunction in a.Functions)
            {
                FunctionPool pool = ScriptPool.Instance.Function;
                ScriptFunction function = pool.GetFunction();
                function.Name = targetFunction.Name;

                foreach (var input in targetFunction.Inputs)
                {
                    FunctionInput functionInput = pool.GetInput();
                    functionInput.Name = input.Name;
                    functionInput.Type = input.Type;

                    function.Inputs.Add(functionInput);
                }

                b.Functions.Add(function);
            }

            foreach (ScriptNode targetNode in a.Nodes)
            {
                ScriptNode node = ScriptNodeFactory.Instance.CreateNode(targetNode.GetType());
                node.Flow = b;
                node.ID = targetNode.ID;
                node.Position = targetNode.Position;

                if (node is FunctionEnterNode functionEnterNode)
                {
                    ScriptFunction function = b.Functions.Find(f => f.Name == functionEnterNode.Function.Name);

                    functionEnterNode.Init(function);
                    function.StartNode = functionEnterNode;
                }

                if (node is FunctionCallNode functionCallNode)
                {
                    ScriptFunction function = b.Functions.Find(f => f.Name == functionCallNode.Function.Name);

                    functionCallNode.Init(function);
                }
                
                foreach (var defaultValue in targetNode.DefaultValues)
                {
                    node.DefaultValues[defaultValue.Key] = defaultValue.Value;
                }


                b.Nodes.Add(node);
                node.Init();
            }

            foreach (NodeConnection targetConnection in a.Connections)
            {
                NodeConnection connection = ScriptPool.Instance.Connection.Get();
                
                connection.SourceID = targetConnection.SourceID;
                connection.SourceKey = targetConnection.SourceKey;
                connection.DestinationID = targetConnection.DestinationID;
                connection.DestinationKey = targetConnection.DestinationKey;
                connection.Flow = b;

                b.Connections.Add(connection);
            }

            b.Load();
        }
    }
}