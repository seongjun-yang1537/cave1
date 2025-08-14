using UnityEngine;

namespace World
{
    [System.Serializable]
    public class SphericalDilationStepAsset : WorldRasterizePipelineStep
    {
        public int radius = 1;
        public int iterations = 1;

        public override IWorldgenStep CreateStep()
        {
            return new SphericalDilationStep(radius, iterations);
        }
    }
}
