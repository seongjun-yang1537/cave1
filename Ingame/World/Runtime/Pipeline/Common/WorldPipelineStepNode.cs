using System.Collections.Generic;
using GraphProcessor;

namespace World.Common
{
    /// <summary>
    /// Base node for a world generation step using GraphProcessor.
    /// </summary>
    public abstract class WorldPipelineStepNode : BaseNode
    {
        protected WorldPipelineStepNode()
        {
            expanded = true;
        }
        [Input(name = "In")]
        public List<WorldPipelineStep> input;

        [Output(name = "Out")]
        public List<WorldPipelineStep> output;

        /// <summary>
        /// Underlying step asset that this node represents.
        /// </summary>
        public abstract WorldPipelineStep Step { get; }

        protected override void Process()
        {
            var steps = input != null ? new List<WorldPipelineStep>(input) : new List<WorldPipelineStep>();
            if (Step != null)
                steps.Add(Step);
            output = steps;
        }
    }
}
