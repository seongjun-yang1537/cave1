using UnityEngine;
using VoxelEngine;

namespace World
{
    public class WorldgenController : MonoBehaviour
    {
        [SerializeField]
        private WorldMap worldMap;

        public void Generate()
        {
            if (worldMap == null) return;
            Worldgen.Generate(worldMap);
        }
    }
}