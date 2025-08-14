using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Rasterize/Add Random Noise")]
    public class AddRandomNoiseStepNode : WorldPipelineStepNode
    {
        public AddRandomNoiseStepAsset step = new AddRandomNoiseStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
