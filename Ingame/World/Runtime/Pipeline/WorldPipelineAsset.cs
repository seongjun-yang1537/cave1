using System.Collections.Generic;
using UnityEngine;

namespace World
{
    [CreateAssetMenu(menuName = "Game/World/World Pipeline Asset", fileName = "WorldPipelineAsset")]
    public class WorldPipelineAsset : ScriptableObject
    {
        public GraphStepGraph graphStep;
        public RasterizeStepGraph rasterizeStep;
        public PopulateStepGraph populateStep;

        public WorldgenPipeline BuildPipeline()
        {
            var pipeline = WorldgenPipeline.Create();

            graphStep?.RefreshSteps();
            rasterizeStep?.RefreshSteps();
            populateStep?.RefreshSteps();

            AppendSteps(pipeline, graphStep?.steps);
            AppendSteps(pipeline, rasterizeStep?.steps);
            AppendSteps(pipeline, populateStep?.steps);

            return pipeline;
        }

        private void AppendSteps(WorldgenPipeline pipeline, IEnumerable<WorldPipelineStep> steps)
        {
            if (steps == null) return;

            foreach (var step in steps)
            {
                if (step == null) continue;
                pipeline.AddStep(step.CreateStep(), step.biomeMask);
            }
        }
    }
}
