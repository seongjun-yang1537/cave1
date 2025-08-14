using UnityEngine;
using UnityEditor;
using GraphProcessor;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UIElements;

namespace World
{
    /// <summary>
    /// Graph window used to edit WorldgenStepGraph assets.
    /// Uses <see cref="AllGraphView"/> as its graph view.
    /// </summary>
    public class WorldgenGraphWindow : BaseGraphWindow
    {
        BaseGraph tmpGraph;
        WorldgenGraphToolbarView toolbarView;

        readonly List<WorldPipelineStep> lastSteps = new();

        protected override void OnEnable()
        {
            base.OnEnable();
            graphLoaded += OnGraphLoaded;
            if (graph != null)
                OnGraphLoaded(graph);
        }

        protected override void OnDisable()
        {
            graphLoaded -= OnGraphLoaded;
            base.OnDisable();
        }

        void OnGraphLoaded(BaseGraph loadedGraph)
        {
            if (loadedGraph is WorldgenStepGraph gpGraph)
            {
                gpGraph.EnsureStartAndEndNodes();
                UpdateTitle(gpGraph);
            }
        }

        void UpdateTitle(BaseGraph graph)
        {
            var path = AssetDatabase.GetAssetPath(graph);
            if (!string.IsNullOrEmpty(path))
            {
                var fileName = Path.GetFileNameWithoutExtension(path);
                titleContent = new GUIContent(fileName)
                {
                    image = AssetDatabase.GetCachedIcon(path) as Texture2D,
                    tooltip = path
                };
            }
            else
            {
                titleContent = new GUIContent("Worldgen Graph");
            }
        }

        [MenuItem("Window/Worldgen/GP Graph Window")]
        public static BaseGraphWindow OpenWithTmpGraph()
        {
            var graphWindow = CreateWindow<WorldgenGraphWindow>();
            graphWindow.tmpGraph = ScriptableObject.CreateInstance<BaseGraph>();
            graphWindow.tmpGraph.hideFlags = HideFlags.HideAndDontSave;
            graphWindow.InitializeGraph(graphWindow.tmpGraph);
            graphWindow.Show();
            return graphWindow;
        }

        protected override void OnDestroy()
        {
            graphView?.Dispose();
            if (tmpGraph != null)
                Object.DestroyImmediate(tmpGraph);
        }

        protected override void InitializeWindow(BaseGraph graph)
        {
            UpdateTitle(graph);

            if (graphView == null)
            {
                graphView = new AllGraphView(this);
                toolbarView = new WorldgenGraphToolbarView(graphView, this);
                graphView.Add(toolbarView);
            }

            rootView.Add(graphView);
        }

        protected override void InitializeGraphView(BaseGraphView view)
        {
        }

        /// <summary>
        /// Execute the current graph and store the resulting populate steps.
        /// </summary>
        public void ExecuteGraph()
        {
            lastSteps.Clear();

            if (graphView?.graph is WorldgenStepGraph gpGraph)
            {
                foreach (var step in gpGraph.EnumerateSteps())
                {
                    lastSteps.Add(step);
                }
            }

            var resultView = graphView.Q<WorldgenGraphResultView>();
            if (resultView != null)
                resultView.SetSteps(lastSteps);
        }

        /// <summary>
        /// Focus the graph view on the start node.
        /// </summary>
        public void FocusStartNode()
        {
            if (graphView?.graph is WorldgenStepGraph gpGraph &&
                gpGraph.startNode != null &&
                graphView.nodeViewsPerNode.TryGetValue(gpGraph.startNode, out var view))
            {
                graphView.ClearSelection();
                graphView.AddToSelection(view);
                graphView.FrameSelection();
            }
        }

        /// <summary>
        /// Focus the graph view on the end node.
        /// </summary>
        public void FocusEndNode()
        {
            if (graphView?.graph is WorldgenStepGraph gpGraph &&
                gpGraph.endNode != null &&
                graphView.nodeViewsPerNode.TryGetValue(gpGraph.endNode, out var view))
            {
                graphView.ClearSelection();
                graphView.AddToSelection(view);
                graphView.FrameSelection();
            }
        }
    }
}
