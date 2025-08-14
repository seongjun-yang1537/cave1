using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Rasterize/Fill Borders")]
    public class FillBordersStepNode : WorldPipelineStepNode
    {
        public FillBordersStepAsset step = new FillBordersStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
