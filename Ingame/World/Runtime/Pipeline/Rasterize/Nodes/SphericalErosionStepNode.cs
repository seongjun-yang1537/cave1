using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Rasterize/Spherical Erosion")]
    public class SphericalErosionStepNode : WorldPipelineStepNode
    {
        public SphericalErosionStepAsset step = new SphericalErosionStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
