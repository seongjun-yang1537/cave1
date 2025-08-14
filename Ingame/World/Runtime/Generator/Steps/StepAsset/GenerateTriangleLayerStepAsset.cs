using UnityEngine;

namespace World
{
    [System.Serializable]
    public class GenerateTriangleLayerStepAsset : WorldPopulatePipelineStep
    {
        public float isolevel = 0.1f;

        public override IWorldgenStep CreateStep()
        {
            return new GenerateTriangleLayerStep(isolevel);
        }
    }
}

