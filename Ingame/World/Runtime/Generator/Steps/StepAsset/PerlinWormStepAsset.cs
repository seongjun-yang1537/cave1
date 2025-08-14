using UnityEngine;

namespace World
{
    [System.Serializable]
    public class PerlinWormStepAsset : WorldRasterizePipelineStep
    {
        public int wormCount = 20;
        public int steps = 100;
        public float stepSize = 1f;
        public float radius = 8f;
        public float radiusVariance = 0.5f;
        public float noiseScale = 0.1f;
        public float branchProbability = 0f;
        public float verticalScale = 0.7f;
        public int branchCount = 0;

        public override IWorldgenStep CreateStep()
        {
            return new PerlinWormStep(wormCount, steps, stepSize, radius, radiusVariance, noiseScale, branchProbability, verticalScale, branchCount);
        }
    }
}
