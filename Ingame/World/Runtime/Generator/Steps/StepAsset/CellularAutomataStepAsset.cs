using UnityEngine;

namespace World
{
    [System.Serializable]
    public class CellularAutomataStepAsset : WorldRasterizePipelineStep
    {
        public int kernelSize = 3;
        [Range(0f, 1f)]
        public float wallThreshold = 0.5f;
        public int iterations = 1;

        public override IWorldgenStep CreateStep()
        {
            return new CellularAutomataStep(kernelSize, wallThreshold, iterations);
        }
    }
}
