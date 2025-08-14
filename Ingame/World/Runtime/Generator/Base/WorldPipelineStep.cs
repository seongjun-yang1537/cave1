using System;
using UnityEngine;

namespace World
{
    [Serializable]
    public abstract class WorldPipelineStep
    {
        public BiomeFlags biomeMask = BiomeFlags.All;
        public abstract IWorldgenStep CreateStep();
    }
}
