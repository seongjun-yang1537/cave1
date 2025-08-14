using UnityEngine;

namespace World
{
    [System.Serializable]
    public class AddPaddingStepAsset : WorldRasterizePipelineStep
    {
        public int padding = 1;
        public override IWorldgenStep CreateStep()
        {
            return new AddPaddingStep(padding);
        }
    }
}
