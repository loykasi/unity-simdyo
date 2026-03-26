using System;
using System.Collections.Generic;

namespace Loykas.Scripting
{
    public interface IScriptNode
    {
        Guid ID { get; set; }
        Dictionary<string, ValueTransfer> DefaultValues { get; set; }
        ScriptFlow Flow { get; set; }

        bool HasOutputTriggers { get; }

        void UpdateNode();
        string GetNameKey();
    }
}