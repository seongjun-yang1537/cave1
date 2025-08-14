using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Rasterize/Connect Rooms MST")]
    public class ConnectRoomsMSTStepNode : WorldPipelineStepNode
    {
        public ConnectRoomsMSTStepAsset step = new ConnectRoomsMSTStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
