using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Rasterize/Perlin Worm")]
    public class PerlinWormStepNode : WorldPipelineStepNode
    {
        public PerlinWormStepAsset step = new PerlinWormStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
