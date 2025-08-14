using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace World
{
    /// <summary>
    /// Custom inspector for <see cref="WorldgenStepGraph"/> allowing execution
    /// of the graph and viewing the resulting pipeline steps directly in the
    /// inspector.
    /// </summary>
    [CustomEditor(typeof(WorldgenStepGraph))]
    public class WorldgenStepGraphEditor : Editor
    {
        readonly List<WorldPipelineStep> steps = new();

        WorldPipelineListDrawer drawer;

        void OnDisable()
        {
            drawer = null;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Execute"))
            {
                ExecuteGraph();
            }

            if (steps.Count > 0)
            {
                EnsureDrawer();
                drawer?.OnGUI();
            }
        }

        void ExecuteGraph()
        {
            steps.Clear();

            var graph = (WorldgenStepGraph)target;
            foreach (var step in graph.EnumerateSteps())
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
