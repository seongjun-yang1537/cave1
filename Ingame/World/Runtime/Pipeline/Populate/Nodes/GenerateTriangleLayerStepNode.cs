using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Populate/Generate Triangle Layer")]
    public class GenerateTriangleLayerStepNode : WorldPipelineStepNode
    {
        public GenerateTriangleLayerStepAsset step = new GenerateTriangleLayerStepAsset();
        public override WorldPipelineStep Step => step;
        public override bool deletable => false;
    }
}
