using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Rasterize/Erosion")]
    public class ErosionStepNode : WorldPipelineStepNode
    {
        public ErosionStepAsset step = new ErosionStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
