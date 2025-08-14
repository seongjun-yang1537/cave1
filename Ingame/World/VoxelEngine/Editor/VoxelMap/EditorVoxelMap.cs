using Corelib.SUI;
using Corelib.Utils;
using UnityEditor;
using UnityEngine;
using Ingame;
using Core;

namespace VoxelEngine
{
    [CustomEditor(typeof(VoxelMap))]
    public class EditorVoxelMap : Editor
    {
        [SerializeField]
        private ChunkedScalarFieldConfig fieldConfig;

        private Editor fieldConfigEditor;
        VoxelMap script;

        protected void OnEnable()
        {
            script = (VoxelMap)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Group("Debug")
                .Content(
                    SEditorGUILayout.Var("Field Config", fieldConfig)
                    + SEditorGUILayout.Action(() =>
                    {
                        if (fieldConfigEditor == null || fieldConfigEditor.target != fieldConfig)
                        {
                            fieldConfigEditor = CreateEditor(fieldConfig);
                        }
                        fieldConfigEditor?.OnInspectorGUI();
                    })
                )
            )
            .Render();
        }
    }
}