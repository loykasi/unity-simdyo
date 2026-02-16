using System;

namespace Loykas.Scripting
{
    public class ScriptNodeContent
    {
        public virtual Type Type => default;
        public ScriptNodeCategory Category;
        public ScriptNode Base;
        public bool ShouldIncludeInMenu;
        public virtual bool CanUseGlobal => true;

        public ScriptNodeContent()
        {
            Base = Create();
        }

        public virtual ScriptNode Create()
        {
            return null;
        }
    }
}