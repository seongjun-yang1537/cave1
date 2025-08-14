using Corelib.Utils;
using UnityEngine;

namespace VoxelEngine
{
    public class BitpackedScalarFieldConfig : ScriptableScalarField<BitpackedScalarField>
    {
        public void CreateOrResize(Vector3Int newSize)
        {
            if (newSize.x <= 0 || newSize.y <= 0 || newSize.z <= 0)
            {
                Debug.LogError("Field size must be positive.");
                return;
            }
            template = new BitpackedScalarField(newSize.x, newSize.y, newSize.z);
        }

        public void GeneratePerlin(int seed = -1, float scale = 0.05f, float height = 15f, float ground = 5f)
        {
            GeneratePerlinTerrain(seed, scale, height, ground);
        }

        public void GenerateRandom(int seed = -1)
        {
            GenerateRandomTerrain(seed);
        }
    }
}