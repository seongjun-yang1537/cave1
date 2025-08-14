using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Populate/Generate Monster Spawn Trigger")]
    public class GenerateMonsterSpawnTriggerStepNode : WorldPipelineStepNode
    {
        public GenerateMonsterSpawnTriggerStepAsset step = new GenerateMonsterSpawnTriggerStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
