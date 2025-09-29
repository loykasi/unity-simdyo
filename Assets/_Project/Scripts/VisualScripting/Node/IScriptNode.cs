using System;
using System.Collections.Generic;

namespace Loykas.Scripting
{
    public interface IScriptNode
    {
        Guid ID { get; set; }
        Dictionary<string, object> DefaultValues { get; set; }
        ScriptFlow Flow { get; set; }

        void UpdateNode();
    }
}