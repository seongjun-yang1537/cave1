using UnityEngine;

namespace World
{
    [System.Serializable]
    public class ConnectRoomsMSTStepAsset : WorldRasterizePipelineStep
    {
        public float stepSize = 1f;
        public float radius = 2f;
        public float radiusVariance = 0.5f;
        public float noiseScale = 0.1f;

        public override IWorldgenStep CreateStep()
        {
            return new ConnectRoomsMSTStep(stepSize, radius, radiusVariance, noiseScale);
        }
    }
}
