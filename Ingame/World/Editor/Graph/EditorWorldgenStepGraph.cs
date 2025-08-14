using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace World
{
    /// <summary>
    /// Custom inspector for WorldgenStepGraph assets that allows executing
    /// the graph and displaying the resulting pipeline steps.
    /// </summary>
    [CustomEditor(typeof(WorldgenStepGraph), true)]
    public class EditorWorldgenStepGraph : Editor
    {
        readonly List<WorldPipelineStep> lastSteps = new();
        WorldPipelineListDrawer drawer;

        void OnDisable()
        {
            drawer = null;
        }

        void EnsureDrawer()
        {
            if (drawer == null)
                drawer = new WorldPipelineListDrawer(lastSteps);
            else
                drawer.SetSteps(lastSteps);
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Space(4);
            if (GUILayout.Button("Open Editor Window"))
            {
                var window = EditorWindow.CreateWindow<WorldgenGraphWindow>();
                window.InitializeGraph((WorldgenStepGraph)target);
                window.Show();
            }

            if (GUILayout.Button("Execute"))
            {
                ExecuteGraph();
            }

            if (lastSteps.Count > 0)
            {
                EnsureDrawer();
                drawer.OnGUI();
            }
        }

        void ExecuteGraph()
        {
            lastSteps.Clear();
            if (target is WorldgenStepGraph gpGraph)
            {
                foreach (var step in gpGraph.EnumerateSteps())
                    lastSteps.Add(step);
            }
            Debug.Log(lastSteps.Count);
        }
    }
}
