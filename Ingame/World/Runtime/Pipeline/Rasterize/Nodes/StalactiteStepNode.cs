using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Rasterize/Stalactite")]
    public class StalactiteStepNode : WorldPipelineStepNode
    {
        public StalactiteStepAsset step = new StalactiteStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
