namespace Loykas.Scripting
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class ScriptNodeAttribute : System.Attribute
    {
        public ScriptNodeCategory Category;
        public bool ShoudlIncludeInMenu;

        public ScriptNodeAttribute(ScriptNodeCategory category, bool includeInMenu = true)
        {
            Category = category;
            ShoudlIncludeInMenu = includeInMenu;
        }
    }
}