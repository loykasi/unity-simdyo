using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Singleton<>), true)]
public class SingletonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("=== Singleton ===", EditorStyles.boldLabel);
        DrawBaseProperties();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("=== Class Fields ===", EditorStyles.boldLabel);
        DrawDerivedProperties();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawBaseProperties()
    {
        SerializedProperty prop = serializedObject.GetIterator();
        prop.NextVisible(true);

        while (prop.NextVisible(false))
        {
            if (IsBaseProperty(prop.name))
                EditorGUILayout.PropertyField(prop, true);
        }
    }

    private void DrawDerivedProperties()
    {
        SerializedProperty prop = serializedObject.GetIterator();
        prop.NextVisible(true);

        while (prop.NextVisible(false))
        {
            if (!IsBaseProperty(prop.name))
                EditorGUILayout.PropertyField(prop, true);
        }
    }

    private bool IsBaseProperty(string propName)
    {
        return serializedObject.targetObject.GetType().BaseType
            .GetField(
                propName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public
            ) != null;
    }
}