using UnityEngine;
using Ingame;
using PathX;

namespace World
{
    [System.Serializable]
    public class SpawnEntityStepAsset : WorldPopulatePipelineStep
    {
        public EntityType entityType = EntityType.None;
        public float poissonRadius = 3f;
        public int maxCount = 10;
        [Range(0f, 1f)] public float spawnProbability = 1f;
        public float triangleRadius = 1f;
        public TriangleDomain domain = TriangleDomain.Ground0;
        public int neighborDepth = 0;
        public int clusterCount = 1;
        public float clusterRadius = 0f;

        public override IWorldgenStep CreateStep()
        {
            return new SpawnEntityStep(entityType, poissonRadius, maxCount, spawnProbability, triangleRadius, domain, biomeMask, clusterCount, clusterRadius, neighborDepth);
        }
    }
}
