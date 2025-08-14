using UnityEngine;

namespace World
{
    [System.Serializable]
    public class GenerateRoomsStepAsset : WorldGraphPipelineStep
    {
        [Range(0f, 1f)]
        public float roomProbability = 1f;

        public override IWorldgenStep CreateStep()
        {
            return new GenerateRoomsStep(null, roomProbability);
        }
    }
}
