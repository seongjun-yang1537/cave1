using UnityEngine;

namespace World
{
    [System.Serializable]
    public class SmoothFieldStepAsset : WorldRasterizePipelineStep
    {
        public override IWorldgenStep CreateStep()
        {
            return new SmoothFieldStep();
        }
    }
}
