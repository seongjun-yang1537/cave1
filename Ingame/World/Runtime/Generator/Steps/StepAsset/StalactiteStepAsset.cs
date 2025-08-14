using UnityEngine;

namespace World
{
    [System.Serializable]
    public class StalactiteStepAsset : WorldRasterizePipelineStep
    {
        [Range(0f, 1f)]
        public float probability = 0.1f;
        public int minLength = 1;
        public int maxLength = 5;

        public override IWorldgenStep CreateStep()
        {
            return new StalactiteStep(probability, minLength, maxLength);
        }
    }
}
