using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Populate/Spawn Entity At Voxels")]
    public class SpawnEntityAtVoxelsStepNode : WorldPipelineStepNode
    {
        public SpawnEntityAtVoxelsStepAsset step = new SpawnEntityAtVoxelsStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
