using UnityEditor;
using UnityEngine;

namespace UI
{
    [CustomPropertyDrawer(typeof(UIMonoBehaviour.ChildEntry))]
    internal class ChildEntryDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent _)
        {
            var nameProp = prop.FindPropertyRelative("fieldName");
            var uiProp = prop.FindPropertyRelative("ui");

            string labelText = $"{nameProp.stringValue} ";

            // GUIContent 하나 새로 만들어서 넘긴다 ― 원본 label 절대 건드리지 말 것
            EditorGUI.PropertyField(pos, uiProp, new GUIContent(labelText), true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("ui"), true);
        }
    }
}