using Corelib.SUI;
using UnityEngine;

namespace PathX
{
    public static class PathXNavProfileConfigInspector
    {
        private static bool fold = false;

        public static SUIElement Render(PathXNavProfileConfig config, bool disabled = false)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUI.DisabledGroup(disabled)
                .Content(
                    SEditorGUILayout.Label($"Vertex : {config.triangles.Count}")
                    + SEditorGUILayout.Var("Chunk Size", config.chunkSize)
                    .OnValueChanged(value => config.chunkSize = value)
                )
            );
        }
    }
}