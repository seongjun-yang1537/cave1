using Corelib.Utils;
using UnityEngine;
using Ingame;
using Core;

namespace VoxelEngine
{
    public class ScriptableScalarField<T> : ScriptableModelConfig<T> where T : IScalarField
    {
        public void GeneratePerlinTerrain(
            int seed = -1,
            float noiseScale = 0.05f,
            float terrainHeightMultiplier = 15f,
            float baseGroundLevel = 5f
            )
        {
            if (template == null) return;
            var rng = (seed == -1) ? GameRng.World : MT19937.Create(seed);
            TerrainGenerator.FillWithPerlinPlains_Job(template, rng, noiseScale, terrainHeightMultiplier, baseGroundLevel);
        }

        public void GenerateRandomTerrain(int seed = -1)
        {
            if (template == null) return;
            var rng = (seed == -1) ? GameRng.World : MT19937.Create(seed);
            TerrainGenerator.FillWithRandom(template, rng);
        }
    }
}