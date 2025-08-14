using System.Collections.Generic;
using UnityEngine.UIElements;
using GraphProcessor;
using UnityEngine;

namespace World
{
    /// <summary>
    /// Pinned window that displays the result of executing a Worldgen GP graph.
    /// </summary>
    public class WorldgenGraphResultView : PinnedElementView
    {
        List<WorldPipelineStep> steps = new List<WorldPipelineStep>();

        WorldPipelineListDrawer drawer;

        public WorldgenGraphResultView()
        {
            title = "Result";
        }

        protected override void Initialize(BaseGraphView graphView)
        {
            scrollable = true;
            UpdateContent();
        }

        void UpdateContent()
        {
            content.Clear();

            if (steps == null || steps.Count == 0)
            {
                content.Add(new Label("No result"));
                return;
            }

            EnsureDrawer();

            var imgui = new IMGUIContainer(() =>
            {
                if (drawer != null)
                    drawer.OnGUI();
            });
            content.Add(imgui);
        }

        void EnsureDrawer()
        {
            if (drawer == null)
                drawer = new WorldPipelineListDrawer(steps);
            else
                drawer.SetSteps(steps);
        }

        public void SetSteps(List<WorldPipelineStep> newSteps)
        {
            steps = newSteps ?? new List<WorldPipelineStep>();
            UpdateContent();
        }

        protected override void Destroy()
        {
            drawer = null;
            base.Destroy();
        }
    }
}
