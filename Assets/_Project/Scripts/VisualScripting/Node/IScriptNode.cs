using System;
using System.Collections.Generic;

public interface IScriptNode
{
    Guid ID { get; set; }
    Dictionary<string, object> DefaultValues { get; set; }
    ScriptFlow Flow { get; set; }
}