using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Populate/Teleport Players To Voxels")]
    public class TeleportPlayersToVoxelsStepNode : WorldPipelineStepNode
    {
        public TeleportPlayersToVoxelsStepAsset step = new TeleportPlayersToVoxelsStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
