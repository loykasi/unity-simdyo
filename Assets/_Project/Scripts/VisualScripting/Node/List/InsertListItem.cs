using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "InsertItem", menuName = "Scriptable Objects/Visual Scripting/Node/List/Insert")]
    public class InsertListItem : ScriptNodeData
    {
        public override ScriptNode Create()
        {
            return new InsertListItemNode(Title);
        }
    }

    class InsertListItemNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue ListInput;
        public InputValue Item;
        public InputValue Index;
        public OutputValue Output;

        public InsertListItemNode(string title) : base(title)
        {
            Enter = InputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any));
            Item = InputValue(nameof(Item));
            Index = InputValue(nameof(Index));
            Output = OutputValue(nameof(Output), ScriptDataType.List(DataType.Any), Get);
        }

        private OutputTrigger Set(ScriptFlow vs)
        {
            IList list = (IList)ListInput.GetValue(vs);
            object item = Item.GetValue(vs);
            int index = (int)(float)Index.GetValue(vs);
            list.Insert(index, item);
            return Exit;
        }

        private object Get(ScriptFlow vs)
        {
            IList list = (IList)ListInput.GetValue(vs);
            return list;
        }
    }
}