using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Populate/Spawn Entity Tables")]
    public class SpawnEntityTablesStepNode : WorldPipelineStepNode
    {
        public SpawnEntityTablesStepAsset step = new SpawnEntityTablesStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
