using UnityEngine;

namespace World
{
    [System.Serializable]
    public class RenderWorldStepAsset : WorldPopulatePipelineStep
    {
        public override IWorldgenStep CreateStep()
        {
            return new RenderWorldStep();
        }
    }
}
