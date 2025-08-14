using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Rasterize/Build Scalar Field From Graph")]
    public class BuildScalarFieldFromGraphStepNode : WorldPipelineStepNode
    {
        public BuildScalarFieldFromGraphStepAsset step = new BuildScalarFieldFromGraphStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
