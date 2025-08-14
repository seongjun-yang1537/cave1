using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Populate/Spawn Entity")]
    public class SpawnEntityStepNode : WorldPipelineStepNode
    {
        public SpawnEntityStepAsset step = new SpawnEntityStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
