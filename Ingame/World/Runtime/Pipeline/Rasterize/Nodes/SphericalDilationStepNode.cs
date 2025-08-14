using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Rasterize/Spherical Dilation")]
    public class SphericalDilationStepNode : WorldPipelineStepNode
    {
        public SphericalDilationStepAsset step = new SphericalDilationStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
