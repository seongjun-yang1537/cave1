using Corelib.SUI;
using UnityEditor;

namespace World
{
    [CustomEditor(typeof(WorldGenerationPreset))]
    public class EditorWorldGenerationPreset : Editor
    {
        private WorldGeneratorWindowPipeline pipelineWindow;

        WorldGenerationPreset script;
        protected virtual void OnEnable()
        {
            script = (WorldGenerationPreset)target;
            pipelineWindow = new WorldGeneratorWindowPipeline(script.pipelineAsset);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SEditorGUI.ChangeCheck(
                target,
                SEditorGUILayout.Vertical()
                .Content(
                    pipelineWindow.Render()
                )
            )
            .Render();
        }
    }
}