using UnityEngine;

namespace World
{
    [CreateAssetMenu(menuName = "Voxel Engine/Cave Generation Preset")]
    public class WorldGenerationPreset : ScriptableObject
    {
        public WorldTheme theme = WorldTheme.Cave;
        public Vector3Int size = new(32, 32, 32);
        public int maxDepth = 6;
        public Vector3Int minCellSize = new(4, 4, 4);
        public int seed = -1;
        [SerializeField]
        public WorldPipelineAsset pipelineAsset;
    }
}
