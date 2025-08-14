using GraphProcessor;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Rasterize/Cellular Automata")]
    public class CellularAutomataStepNode : WorldPipelineStepNode
    {
        public CellularAutomataStepAsset step = new CellularAutomataStepAsset();
        public override WorldPipelineStep Step => step;
    }
}
