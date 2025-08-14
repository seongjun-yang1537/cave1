using UnityEngine;

namespace World
{
    [System.Serializable]
    public class GenerateVSPTreeStepAsset : WorldGraphPipelineStep
    {
        public override IWorldgenStep CreateStep()
        {
            return new GenerateVSPTreeStep();
        }
    }
}
