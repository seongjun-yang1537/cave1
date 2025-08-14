using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Populate/Generate PathX NavMesh")]
    public class GeneratePathXNavMeshStepNode : WorldPipelineStepNode
    {
        public GeneratePathXNavMeshStepAsset step = new GeneratePathXNavMeshStepAsset();
        public override WorldPipelineStep Step => step;
        public override bool deletable => false;
    }
}
