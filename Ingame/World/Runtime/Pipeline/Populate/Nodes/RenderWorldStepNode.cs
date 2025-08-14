using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Populate/Render World")]
    public class RenderWorldStepNode : WorldPipelineStepNode
    {
        public RenderWorldStepAsset step = new RenderWorldStepAsset();
        public override WorldPipelineStep Step => step;
        public override bool deletable => false;
    }
}
