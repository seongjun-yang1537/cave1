using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace World
{
    [CustomEditor(typeof(WorldPipelineAsset))]
    public class WorldPipelineAssetEditor : Editor
    {
        WorldPipelineAsset asset;

        List<WorldPipelineStep> steps = new List<WorldPipelineStep>();
        WorldPipelineListDrawer drawer;

        void OnEnable()
        {
            asset = (WorldPipelineAsset)target;
        }

        void OnDisable()
        {
            drawer = null;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("graphStep"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rasterizeStep"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("populateStep"));
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Execute"))
            {
                ExecuteGraph();
            }

            if (steps.Count > 0)
            {
                EnsureDrawer();
                drawer.OnGUI();
            }
        }

        void ExecuteGraph()
        {
            steps.Clear();
            asset.graphStep?.RefreshSteps();
            asset.rasterizeStep?.RefreshSteps();
            asset.populateStep?.RefreshSteps();
            AppendSteps(asset.graphStep?.steps);
            AppendSteps(asset.rasterizeStep?.steps);
            AppendSteps(asset.populateStep?.steps);
        }

        void AppendSteps(IEnumerable<WorldPipelineStep> s)
        {
            if (s == null) return;
            foreach (var step in s)
                if (step != null)
                    steps.Add(step);
        }

        void EnsureDrawer()
        {
            if (drawer == null)
                drawer = new WorldPipelineListDrawer(steps);
            else
                drawer.SetSteps(steps);
        }
    }
}
