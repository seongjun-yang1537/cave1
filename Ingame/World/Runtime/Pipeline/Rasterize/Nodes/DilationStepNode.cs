using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Rasterize/Dilation")]
    public class DilationStepNode : WorldPipelineStepNode
    {
        public DilationStepAsset step = new DilationStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
