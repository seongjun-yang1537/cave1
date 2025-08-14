using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UI
{
    [CustomPropertyDrawer(typeof(DynamicUIPrefabAttribute))]
    public class DynamicUIPrefabDrawer : PropertyDrawer
    {
        DynamicUITable table;
        string[] options = Array.Empty<string>();
        int selectedIndex;

        void EnsureLoaded()
        {
            if (table != null) return;
            table = Resources.Load<DynamicUITable>("UI/DynamicUITable");
            if (table != null)
                options = table.table.Keys.ToArray();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EnsureLoaded();

            float line = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            Rect dropdownRect = new Rect(position.x, position.y, position.width - 60f, line);
            Rect buttonRect = new Rect(dropdownRect.xMax + 2f, position.y, 58f, line);
            Rect fieldRect = new Rect(position.x, position.y + line + spacing, position.width, line);
            Rect helpRect = new Rect(position.x, fieldRect.yMax + spacing, position.width, line);

            if (options.Length > 0)
            {
                selectedIndex = EditorGUI.Popup(dropdownRect, selectedIndex, options);
                if (GUI.Button(buttonRect, "Apply"))
                {
                    string key = options[selectedIndex];
                    var prefab = table[key];
                    property.objectReferenceValue = prefab;

                    property.serializedObject.ApplyModifiedProperties();
                }
            }
            else
            {
                EditorGUI.LabelField(dropdownRect, "DynamicUITable not found");
            }

            EditorGUI.PropertyField(fieldRect, property, label);

            if (property.objectReferenceValue == null)
                EditorGUI.HelpBox(helpRect, $"{property.displayName} is required", MessageType.Error);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
            if (property.objectReferenceValue == null)
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            return height;
        }
    }
}
