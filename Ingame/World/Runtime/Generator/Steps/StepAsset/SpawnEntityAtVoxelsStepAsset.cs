using UnityEngine;
using Ingame;
using PathX;

namespace World
{
    [System.Serializable]
    public class SpawnEntityAtVoxelsStepAsset : WorldPopulatePipelineStep
    {
        public EntityType entityType = EntityType.None;
        public bool spawnAtStart = true;
        public bool spawnAtEnd = true;
        public TriangleDomain domain = TriangleDomain.Ground0;

        public override IWorldgenStep CreateStep()
        {
            return new SpawnEntityAtVoxelsStep(entityType, spawnAtStart, spawnAtEnd, domain);
        }
    }
}
