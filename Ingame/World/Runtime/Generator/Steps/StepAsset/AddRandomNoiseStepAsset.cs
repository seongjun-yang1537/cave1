using UnityEngine;

namespace World
{
    [System.Serializable]
    public class AddRandomNoiseStepAsset : WorldRasterizePipelineStep
    {
        [Range(0f, 1f)]
        public float noisePercent = 0.4f;

        public override IWorldgenStep CreateStep()
        {
            return new AddRandomNoiseStep(noisePercent);
        }
    }
}
