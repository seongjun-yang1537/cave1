using GraphProcessor;
using UnityEngine;
using World.Common;

namespace World
{
    [System.Serializable, NodeMenuItem("World/Graph/Paint Biomes")]
    public class PaintBiomesStepNode : WorldPipelineStepNode
    {
        [SerializeField]
        public PaintBiomesStepAsset step;
        public override WorldPipelineStep Step => step;
    }
}
