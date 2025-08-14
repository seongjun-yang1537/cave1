using UnityEngine;

namespace World
{
    [System.Serializable]
    public class SphericalErosionStepAsset : WorldRasterizePipelineStep
    {
        public int radius = 1;
        public int iterations = 1;

        public override IWorldgenStep CreateStep()
        {
            return new SphericalErosionStep(radius, iterations);
        }
    }
}
