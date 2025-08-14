using Corelib.SUI;
using UnityEditor;
using UnityEngine;

namespace VoxelEngine
{
    [CustomEditor(typeof(ScalarFieldMeshRenderer))]
    public class EditorScalarFieldMeshRenderer : Editor
    {
        private Editor configEditor;

        ScalarFieldMeshRenderer script;

        SerializedProperty bitpackedProp;
        SerializedProperty chunkedProp;

        int selectedTab = 0;
        string[] tabLabels = { "Bitpacked", "Chunked" };

        void OnEnable()
        {
            script = (ScalarFieldMeshRenderer)target;
            bitpackedProp = serializedObject.FindProperty("bitpackedScalarField");
            chunkedProp = serializedObject.FindProperty("chunkScalarField");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Scalar Field Configuration", EditorStyles.boldLabel);
            selectedTab = GUILayout.Toolbar(selectedTab, tabLabels);

            EditorGUILayout.Space();

            switch (selectedTab)
            {
                case 0:
                    EditorGUILayout.PropertyField(bitpackedProp, new GUIContent("Bitpacked Config"));
                    chunkedProp.objectReferenceValue = null;
                    break;
                case 1:
                    EditorGUILayout.PropertyField(chunkedProp, new GUIContent("Chunked Config"));
                    bitpackedProp.objectReferenceValue = null;
                    break;
            }

            EditorGUILayout.Space(10);

            var renderer = (ScalarFieldMeshRenderer)target;
            var field = renderer.scalarField;

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Selected Scalar Field", field as Object, typeof(Object), false);
            EditorGUI.EndDisabledGroup();

            SEditorGUILayout.Vertical()
            .Content(
                RenderConfig()
            )
            .Render();

            serializedObject.ApplyModifiedProperties();
        }

        private SUIElement RenderConfig()
        {
            return SEditorGUILayout.Group("Config")
            .Content(
                SEditorGUILayout.Action(() =>
                {
                    if (configEditor == null)
                    {
                        if (script.bitpackedScalarField != null && configEditor?.target != script.bitpackedScalarField)
                        {
                            configEditor = CreateEditor(script.bitpackedScalarField);
                        }
                        if (script.chunkScalarField != null && configEditor?.target != script.chunkScalarField)
                        {
                            configEditor = CreateEditor(script.chunkScalarField);
                        }
                    }

                    if (configEditor == null) return;
                    configEditor.OnInspectorGUI();
                })
            );
        }
    }
}