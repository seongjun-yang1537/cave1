using Corelib.SUI;
using Corelib.Utils;
using UnityEditor;
using UnityEngine;

namespace Realm
{
    [CustomEditor(typeof(RealmConfigTable))]
    public class EditorRealmConfigTable : Editor
    {
        RealmConfigTable script;
        protected virtual void OnEnable()
        {
            script = (RealmConfigTable)target;
        }

        public override void OnInspectorGUI()
        {
            SEditorGUI.ChangeCheck(
                target,
                SEditorGUILayout.Vertical()
                .Content(
                    RenderInput()
                    + SEditorGUILayout.Separator()
                    + RenderConfigList()
                    + SEditorGUILayout.Separator()
                    + SEditorGUILayout.HelpBox(
                        script.ValidateRanges() ? "Ranges Valid" : "Ranges Invalid",
                        script.ValidateRanges() ? MessageType.Info : MessageType.Error
                    )
                    + SEditorGUILayout.Button("Sort")
                        .OnClick(() => { script.SortConfigs(); EditorUtility.SetDirty(target); })
                )
            )
            .Render();
        }

        FloatRange inputDepthRange = new(0f, 0f);

        private SUIElement RenderInput()
        {
            return SEditorGUILayout.Horizontal()
            .LabelWidth(60f)
            .Content(
                SEditorGUILayout.Label("New Config")
                .Width(80)
                + SEditorGUILayout.Var("", inputDepthRange.Min)
                .OnValueChanged(value => inputDepthRange.Min = value)
                + SEditorGUILayout.Label("~")
                .Width(8)
                + SEditorGUILayout.Var("", inputDepthRange.Max)
                .OnValueChanged(value => inputDepthRange.Max = value)
                + SEditorGUILayout.Button("+")
                .Width(16)
                .OnClick(() =>
                {
                    RealmDepthConfig newConfig = new RealmDepthConfig();
                    newConfig.depthRange = new FloatRange(inputDepthRange);
                    script.configs.Add(newConfig);
                })
            );
        }

        private SUIElement RenderConfigList()
        {
            EnsureConfigList();
            SUIElement content = SUIElement.Empty();

            for (int i = 0; i < script.configs.Count; i++)
            {
                var config = script.configs[i];
                int currentIndex = i;

                content += SEditorGUILayout.Vertical("HelpBox")
                .Content(
                    SEditorGUILayout.Label("Config")
                    .Align(TextAnchor.MiddleCenter)
                    .Bold()
                    + SEditorGUILayout.Horizontal()
                    .LabelWidth(60f)
                    .Content(
                        SEditorGUILayout.Label("Depth")
                        .Width(64)
                        + SEditorGUILayout.Var("", config.depthRange.Min)
                        .OnValueChanged(value => config.depthRange.Min = value)
                        + SEditorGUILayout.Label("~")
                        .Width(8)
                        + SEditorGUILayout.Var("", config.depthRange.Max)
                        .OnValueChanged(value => config.depthRange.Max = value)
                        + SEditorGUILayout.Button("-")
                        .Width(16)
                        .OnClick(() => script.configs.RemoveAt(currentIndex))
                    )
                    + SEditorGUILayout.Separator()
                    + RealmDepthConfigInspector.Render(config)
                );
            }

            return content;
        }

        private void EnsureConfigList()
        {
            if (script.configs == null)
                script.configs = new();
        }
    }
}