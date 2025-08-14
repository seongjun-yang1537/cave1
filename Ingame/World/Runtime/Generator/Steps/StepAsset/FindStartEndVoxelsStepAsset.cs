using UnityEngine;

namespace World
{
    [System.Serializable]
    public class FindStartEndVoxelsStepAsset : WorldRasterizePipelineStep
    {
        public Vector3 requiredSpace = new Vector3(3f, 3f, 3f);
        public int samplesPerRoom = 8;
        public float carveRadius = 2f;

        public override IWorldgenStep CreateStep()
        {
            return new FindStartEndVoxelsStep(requiredSpace, samplesPerRoom, carveRadius);
        }
    }
}
