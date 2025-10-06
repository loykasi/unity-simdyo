using System;

namespace Loykas.Scripting
{
    public class ScriptNodeContent
    {
        public virtual Type Type => default;
        public ScriptNodeCategory Category;

        public virtual ScriptNode Create()
        {
            return null;
        }
    }
}