using System.Collections.Generic;
using System.Linq;
using GraphProcessor;

namespace World.Common
{
    [System.Serializable]
    public class WorldgenStepEndNode : BaseNode
    {
        [Input(name = "In")]
        public List<WorldPipelineStep> input;

        public override bool deletable => false;

        protected override void Process() { }

        [GraphProcessor.IsCompatibleWithGraph]
        public static bool CanCreate(BaseGraph graph)
        {
            if (graph is World.WorldgenStepGraph g)
                return !g.nodes.OfType<WorldgenStepEndNode>().Any();
            return false;
        }
    }
}
