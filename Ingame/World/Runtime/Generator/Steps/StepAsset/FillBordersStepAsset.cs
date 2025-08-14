using UnityEngine;

namespace World
{
    [System.Serializable]
    public class FillBordersStepAsset : WorldRasterizePipelineStep
    {
        [Min(1)]
        public int borderSize = 1;

        public override IWorldgenStep CreateStep()
        {
            return new FillBordersStep(borderSize);
        }
    }
}
