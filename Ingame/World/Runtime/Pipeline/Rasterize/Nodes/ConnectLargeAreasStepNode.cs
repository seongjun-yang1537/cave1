using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Rasterize/Connect Large Areas")]
    public class ConnectLargeAreasStepNode : WorldPipelineStepNode
    {
        public ConnectLargeAreasStepAsset step = new ConnectLargeAreasStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
