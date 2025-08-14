using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Rasterize/Find Start End Voxels")]
    public class FindStartEndVoxelsStepNode : WorldPipelineStepNode
    {
        public FindStartEndVoxelsStepAsset step = new FindStartEndVoxelsStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
