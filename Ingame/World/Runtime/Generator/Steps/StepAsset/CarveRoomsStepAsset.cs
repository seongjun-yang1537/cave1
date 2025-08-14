using UnityEngine;

namespace World
{
    [System.Serializable]
    public class CarveRoomsStepAsset : WorldRasterizePipelineStep
    {
        public float noiseScale = 0.1f;
        public float radiusVariance = 10f;
        public int minSpheres = 2;
        public int maxSpheres = 4;

        public override IWorldgenStep CreateStep()
        {
            return new CarveRoomsStep(noiseScale, radiusVariance, minSpheres, maxSpheres);
        }
    }
}

