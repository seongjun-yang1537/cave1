using UnityEngine;

namespace World
{
    [System.Serializable]
    public class BuildScalarFieldFromGraphStepAsset : WorldRasterizePipelineStep
    {
        public int corridorSize = 3;

        public override IWorldgenStep CreateStep()
        {
            return new BuildScalarFieldFromGraphStep(corridorSize);
        }
    }
}
