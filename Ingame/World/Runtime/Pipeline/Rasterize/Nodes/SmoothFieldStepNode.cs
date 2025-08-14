using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Rasterize/Smooth Field")]
    public class SmoothFieldStepNode : WorldPipelineStepNode
    {
        public SmoothFieldStepAsset step = new SmoothFieldStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
