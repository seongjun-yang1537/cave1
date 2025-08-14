using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Rasterize/Add Padding")]
    public class AddPaddingStepNode : WorldPipelineStepNode
    {
        public AddPaddingStepAsset step = new AddPaddingStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
