using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Rasterize/Carve Rooms")]
    public class CarveRoomsStepNode : WorldPipelineStepNode
    {
        public CarveRoomsStepAsset step = new CarveRoomsStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
