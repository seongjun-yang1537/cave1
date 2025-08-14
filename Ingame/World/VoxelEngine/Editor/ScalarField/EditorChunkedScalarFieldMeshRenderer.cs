using Corelib.SUI;
using UnityEditor;

namespace VoxelEngine
{
    [CustomEditor(typeof(ChunkedScalarFieldMeshRenderer), true)]
    public class EditorChunkedScalarFieldMeshRenderer : Editor
    {
        ChunkedScalarFieldMeshRenderer script;

        protected virtual void OnEnable()
        {
            script = (ChunkedScalarFieldMeshRenderer)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Separator()
                    + SEditorGUILayout.Label("Actions").Bold()
                    + SEditorGUILayout.Vertical("box")
                        .Content(
                            SEditorGUILayout.Button("Clear All Meshes")
                                .OnClick(() => script.ClearAllMeshes())
                        )
                )
                .Render();
        }
    }
}
