using Corelib.SUI;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using World;

namespace Realm
{
    [CustomEditor(typeof(RealmGenerationProfile))]
    public class EditorRealmGenerationProfile : Editor
    {
        private ReorderableList profileList;

        RealmGenerationProfile script;

        private void OnEnable()
        {
            script = (RealmGenerationProfile)target;

            var profilesProp = serializedObject.FindProperty("profiles");
            profileList = new ReorderableList(serializedObject, profilesProp, true, true, true, true);

            profileList.drawHeaderCallback = rect => GUI.Label(rect, "Realm Profiles");

            profileList.drawElementCallback = (rect, index, active, focused) =>
            {
                var element = profilesProp.GetArrayElementAtIndex(index);
                var presetProp = element.FindPropertyRelative("generationPreset");
                var minProp = element.FindPropertyRelative("Min");
                var maxProp = element.FindPropertyRelative("Max");

                var nextDepthRangeProp = element.FindPropertyRelative("nextDepthRange");
                var nextMinProp = nextDepthRangeProp.FindPropertyRelative("Min");
                var nextMaxProp = nextDepthRangeProp.FindPropertyRelative("Max");

                float lineHeight = EditorGUIUtility.singleLineHeight;
                float spacing = 2f;
                float labelWidth = 110f;
                float fieldSpacing = 4f;

                // ───── preset ─────
                Rect presetLabel = new Rect(rect.x, rect.y, labelWidth, lineHeight);
                Rect presetField = new Rect(rect.x + labelWidth + fieldSpacing, rect.y, rect.width - labelWidth - fieldSpacing, lineHeight);
                EditorGUI.LabelField(presetLabel, "Generation Preset");
                EditorGUI.PropertyField(presetField, presetProp, GUIContent.none);

                // ───── depth range ─────
                Rect depthLabel = new Rect(rect.x, rect.y + lineHeight + spacing, labelWidth, lineHeight);
                float fieldWidth = (rect.width - labelWidth - fieldSpacing - 12f) / 2;
                Rect minRect = new Rect(rect.x + labelWidth + fieldSpacing, rect.y + lineHeight + spacing, fieldWidth, lineHeight);
                Rect tildeRect1 = new Rect(minRect.xMax + 2, minRect.y, 8, lineHeight);
                Rect maxRect = new Rect(tildeRect1.xMax + 2, minRect.y, fieldWidth, lineHeight);

                EditorGUI.LabelField(depthLabel, "Depth Range");
                EditorGUI.PropertyField(minRect, minProp, GUIContent.none);
                EditorGUI.LabelField(tildeRect1, "~");
                EditorGUI.PropertyField(maxRect, maxProp, GUIContent.none);

                // ───── next depth range ─────
                Rect nextLabel = new Rect(rect.x, rect.y + (lineHeight + spacing) * 2, labelWidth, lineHeight);
                Rect nextMinRect = new Rect(rect.x + labelWidth + fieldSpacing, nextLabel.y, fieldWidth, lineHeight);
                Rect tildeRect2 = new Rect(nextMinRect.xMax + 2, nextMinRect.y, 8, lineHeight);
                Rect nextMaxRect = new Rect(tildeRect2.xMax + 2, nextMinRect.y, fieldWidth, lineHeight);

                EditorGUI.LabelField(nextLabel, "Next Depth Range");
                EditorGUI.PropertyField(nextMinRect, nextMinProp, GUIContent.none);
                EditorGUI.LabelField(tildeRect2, "~");
                EditorGUI.PropertyField(nextMaxRect, nextMaxProp, GUIContent.none);
            };

            profileList.elementHeightCallback = _ => EditorGUIUtility.singleLineHeight * 3 + 8;

            profileList.onAddCallback = l =>
            {
                l.serializedProperty.arraySize++;
                var elem = l.serializedProperty.GetArrayElementAtIndex(l.serializedProperty.arraySize - 1);
                elem.FindPropertyRelative("Min").floatValue = 0f;
                elem.FindPropertyRelative("Max").floatValue = 0f;
                elem.FindPropertyRelative("generationPreset").objectReferenceValue = null;

                var nextDepthRangeProp = elem.FindPropertyRelative("nextDepthRange");
                nextDepthRangeProp.FindPropertyRelative("Min").floatValue = 0f;
                nextDepthRangeProp.FindPropertyRelative("Max").floatValue = 0f;
            };

            profileList.onChangedCallback = l => { serializedObject.ApplyModifiedProperties(); EditorUtility.SetDirty(target); };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SEditorGUI.ChangeCheck(
                target,
                SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Var("Testbed Preset", script.testbedPreset)
                    .OnValueChanged(value => script.testbedPreset = value as WorldGenerationPreset)
                    + SEditorGUILayout.Action(() => profileList.DoLayoutList())
                    + SEditorGUILayout.Button("Sort")
                    .OnClick(() => script.SortProfiles())
                )
            )
            .Render();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
