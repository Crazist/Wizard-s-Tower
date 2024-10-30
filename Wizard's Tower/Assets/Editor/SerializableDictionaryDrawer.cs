using Enemy.Sequence.Logic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(SerializableDictionary))]
    public class SerializableDictionaryDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty entries = property.FindPropertyRelative("_entries");
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        
            if (entries != null)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < entries.arraySize; i++)
                {
                    var entry = entries.GetArrayElementAtIndex(i);
                    var key = entry.FindPropertyRelative("Key");
                    var actions = entry.FindPropertyRelative("Actions");

                    EditorGUILayout.BeginHorizontal();
                    key.stringValue = EditorGUILayout.TextField("Key", key.stringValue);
                    EditorGUILayout.PropertyField(actions, new GUIContent("Actions"), true);
                    if (GUILayout.Button("Remove"))
                    {
                        entries.DeleteArrayElementAtIndex(i);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("Add New Entry"))
                {
                    entries.InsertArrayElementAtIndex(entries.arraySize);
                }
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 
            EditorGUI.GetPropertyHeight(property, label, true);
    }
}