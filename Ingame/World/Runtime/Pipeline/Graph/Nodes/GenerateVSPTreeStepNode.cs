using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Graph/Generate VSP Tree")]
    public class GenerateVSPTreeStepNode : WorldPipelineStepNode
    {
        public GenerateVSPTreeStepAsset step = new GenerateVSPTreeStepAsset();
        public override WorldPipelineStep Step => step;
        public override bool deletable => false;
    }
}
