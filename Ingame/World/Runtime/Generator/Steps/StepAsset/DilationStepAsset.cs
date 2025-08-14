using UnityEngine;

namespace World
{
    [System.Serializable]
    public class DilationStepAsset : WorldRasterizePipelineStep
    {
        public int iterations = 1;
        public int kernelSize = 5;

        public override IWorldgenStep CreateStep()
        {
            return new CubicDilationStep(iterations, kernelSize);
        }
    }
}
