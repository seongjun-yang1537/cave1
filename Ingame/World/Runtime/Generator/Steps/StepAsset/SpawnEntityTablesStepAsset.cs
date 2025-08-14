using System;
using System.Collections.Generic;
using UnityEngine;
using Ingame;
using PathX;

namespace World
{
    [System.Serializable]
    public class SpawnEntityTablesStepAsset : WorldPopulatePipelineStep
    {
        [Serializable]
    public class Entry
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
        }

        public List<Entry> entries = new();

        public override IWorldgenStep CreateStep()
        {
            return new SpawnEntityTablesStep(entries, biomeMask);
        }
    }
}

