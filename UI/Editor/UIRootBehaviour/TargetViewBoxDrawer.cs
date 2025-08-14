using UnityEditor;
using UnityEngine;

namespace UI
{
    [CustomPropertyDrawer(typeof(TargetViewBoxAttribute))]
    public class TargetViewBoxDrawer : PropertyDrawer
    {
        private static readonly GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 11,
            fontStyle = FontStyle.Bold
        };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float boxPadding = 4f;

            bool isAssigned = property.objectReferenceValue != null;
            float helpHeight = isAssigned ? 0f : lineHeight * 2f;

            // 전체 높이 계산
            float totalHeight = lineHeight + spacing + lineHeight + spacing + helpHeight + spacing;

            // 박스
            Rect boxRect = new Rect(position.x, position.y, position.width, totalHeight);
            GUI.Box(boxRect, GUIContent.none);

            // [UI Component] 타이틀
            Rect titleRect = new Rect(position.x, position.y + boxPadding, position.width, lineHeight);
            EditorGUI.LabelField(titleRect, "[UI Root Behaviour]", titleStyle);

            // 필드
            Rect fieldRect = new Rect(position.x + boxPadding, titleRect.yMax + spacing, position.width - boxPadding * 2, lineHeight);
            EditorGUI.PropertyField(fieldRect, property, new GUIContent(property.displayName));

            // HelpBox (값이 비어있을 때)
            if (!isAssigned)
            {
                Rect helpRect = new Rect(position.x + boxPadding, fieldRect.yMax + spacing, position.width - boxPadding * 2, helpHeight);
                EditorGUI.HelpBox(helpRect, "⚠ targetView is not assigned!", MessageType.Warning);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            bool isAssigned = property.objectReferenceValue != null;
            float helpHeight = isAssigned ? 0f : lineHeight * 2f;

            return lineHeight + spacing + lineHeight + spacing + helpHeight + spacing;
        }
    }
}