using Corelib.SUI;
using UnityEditor;
using UnityEngine;

namespace PathX
{
    public static class PathXEngineInspector
    {
        public static SUIElement Render(PathXEngine engine)
        {
            if (engine == null) return SUIElement.Empty();

            return SEditorGUILayout.Group("Path X Engine")
            .Content(
                RenderEngineStatus(!engine.NeedsReload)
                + RenderProfiles(engine)
            );
        }

        private static SUIElement RenderProfiles(PathXEngine engine)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Action(() =>
                {
                    foreach (var agentProfile in engine.Profiles)
                    {
                        PathxNavProfileInspector.Render(agentProfile)
                            .Render();
                    }
                })
            );
        }

        private static SUIElement RenderEngineStatus(bool isLoaded)
        {
            GUIStyle boxStyle = new GUIStyle(EditorStyles.helpBox)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 13,
                fontStyle = FontStyle.Bold,
                padding = new RectOffset(10, 10, 10, 10),
                richText = true
            };

            GUIContent icon = EditorGUIUtility.IconContent("d_SettingsIcon");

            string label = isLoaded
                ? "<color=#32CD32><b>[ENGINE ONLINE]</b></color>  Core system is active."
                : "<color=#D9534F><b>[ENGINE OFFLINE]</b></color>  Core system not initialized.";

            Color bgColor = isLoaded ? new Color(0.15f, 0.25f, 0.15f) : new Color(0.25f, 0.05f, 0.05f);
            Color prevColor = GUI.backgroundColor;

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Action(() => GUI.backgroundColor = bgColor)
                + SEditorGUILayout.Vertical(boxStyle)
                .Content(
                    SEditorGUILayout.Horizontal()
                    .Content(
                        SEditorGUILayout.Action(() =>
                        {
                            GUILayout.Label(icon, GUILayout.Width(24), GUILayout.Height(24));
                            EditorGUILayout.LabelField(new GUIContent(label), new GUIStyle(EditorStyles.label) { richText = true });
                        })
                    )
                )
                + SEditorGUILayout.Action(() => GUI.backgroundColor = prevColor)
            );
        }
    }
}