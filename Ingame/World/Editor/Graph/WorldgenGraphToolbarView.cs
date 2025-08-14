using GraphProcessor;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace World
{
    /// <summary>
    /// Toolbar for <see cref="WorldgenGraphWindow"/>.
    /// </summary>
    public class WorldgenGraphToolbarView : ToolbarView
    {
        private readonly WorldgenGraphWindow window;

        public WorldgenGraphToolbarView(BaseGraphView graphView, WorldgenGraphWindow window) : base(graphView)
        {
            this.window = window;
        }

        protected override void AddButtons()
        {
            AddButton("Execute", window.ExecuteGraph);
            AddButton("Result Window", () => graphView.ToggleView<WorldgenGraphResultView>());
            AddButton("Find Start Node", window.FocusStartNode);
            AddButton("Find End Node", window.FocusEndNode);
            AddCustom(DrawStats, left: false);
            base.AddButtons();
        }

        void DrawStats()
        {
            var g = graphView?.graph;
            int nodeCount = g?.nodes?.Count ?? 0;
            int componentSize = 0;
            if (g is WorldgenStepGraph gpGraph)
                componentSize = gpGraph.EnumerateSteps().Count();

            GUILayout.Label($"Nodes: {nodeCount}  Component: {componentSize}", EditorStyles.miniLabel);
        }
    }
}
