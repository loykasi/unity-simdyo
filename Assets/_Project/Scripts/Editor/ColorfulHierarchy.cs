using UnityEditor;
using UnityEngine;

namespace GameCore.Editor
{
    [InitializeOnLoad]
    public static class ColorfulHierarchy
    {
        static ColorfulHierarchy()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }

        private static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            var gameObject = EditorUtility.InstanceIDToObject(instanceID);
            if (gameObject != null && gameObject.name.StartsWith("/", System.StringComparison.Ordinal))
            {
                EditorGUI.DrawRect(selectionRect, Color.gray);
                EditorGUI.DropShadowLabel(selectionRect, gameObject.name.Replace("/", "").ToUpperInvariant());
            }
        }
    }
}