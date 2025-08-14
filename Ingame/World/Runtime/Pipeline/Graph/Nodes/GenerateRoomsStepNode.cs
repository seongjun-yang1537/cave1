using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Graph/Generate Rooms")]
    public class GenerateRoomsStepNode : WorldPipelineStepNode
    {
        public GenerateRoomsStepAsset step = new GenerateRoomsStepAsset();
        public override WorldPipelineStep Step => step;
        public override bool deletable => false;
    }
}
