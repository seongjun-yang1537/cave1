using UnityEngine;
using Ingame;
using PathX;

namespace World
{
    [System.Serializable]
    public class GenerateMonsterSpawnTriggerStepAsset : WorldPopulatePipelineStep
    {
        public float poissonRadius = 5f;
        public int maxCount = 10;
        public float triggerRadius = 3f;
        public MonsterProbabilityTable probabilityTable;
        public TriangleDomain domain = TriangleDomain.Ground0;

        public override IWorldgenStep CreateStep()
        {
            return new GenerateMonsterSpawnTriggerStep(poissonRadius, maxCount, triggerRadius, probabilityTable, domain);
        }
    }
}
