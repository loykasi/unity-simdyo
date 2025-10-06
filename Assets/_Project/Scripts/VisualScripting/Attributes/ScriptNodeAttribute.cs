using System;

namespace Loykas.Scripting
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class ScriptNodeAttribute : System.Attribute
    {
        public ScriptNodeCategory Category;

        public ScriptNodeAttribute(ScriptNodeCategory category)
        {
            Category = category;
        }
    }
}