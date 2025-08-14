using UnityEngine;

namespace World
{
    [System.Serializable]
    public class ErosionStepAsset : WorldRasterizePipelineStep
    {
        public int iterations = 1;
        public int kernelSize = 5;

        public override IWorldgenStep CreateStep()
        {
            return new CubicErosionStep(iterations, kernelSize);
        }
    }
}
