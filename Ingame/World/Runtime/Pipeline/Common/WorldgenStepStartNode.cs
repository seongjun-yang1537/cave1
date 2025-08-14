using System.Collections.Generic;
using System.Linq;
using GraphProcessor;

namespace World.Common
{
    [System.Serializable]
    public class WorldgenStepStartNode : BaseNode
    {
        [Output(name = "Out")]
        public List<WorldPipelineStep> output;

        public override bool deletable => false;

        private List<WorldPipelineStep> injectedSteps;

        public void SetInputSteps(List<WorldPipelineStep> steps)
        {
            injectedSteps = steps;
        }

        protected override void Process()
        {
            output = injectedSteps;
        }

        [GraphProcessor.IsCompatibleWithGraph]
        public static bool CanCreate(BaseGraph graph)
        {
            if (graph is World.WorldgenStepGraph g)
                return !g.nodes.OfType<WorldgenStepStartNode>().Any();
            return false;
        }
    }
}
