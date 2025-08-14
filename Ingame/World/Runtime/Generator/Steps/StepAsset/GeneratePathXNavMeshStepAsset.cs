using UnityEngine;

namespace World
{
    [System.Serializable]
    public class GeneratePathXNavMeshStepAsset : WorldPopulatePipelineStep
    {
        public int chunkSize = 2;

        public override IWorldgenStep CreateStep()
        {
            return new GeneratePathXNavMeshStep(chunkSize);
        }
    }
}
