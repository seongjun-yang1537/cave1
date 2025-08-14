using UnityEngine;

namespace World
{
    [CreateAssetMenu(fileName = "New Biome Material Data", menuName = "Game/World/Biome Material Data")]
    public class BiomeMaterialData : ScriptableObject
    {
        [Header("Biome Identification")]
        public BiomeType biomeType;

        [Header("Top/Bottom Textures (Y-axis)")]
        public Texture2D y_Texture;
        public Texture2D y_NormalMap;

        [Header("Side Textures (XZ-plane)")]
        public Texture2D xz_Texture;
        public Texture2D xz_NormalMap;

        [Header("Material Properties")]
        [Tooltip("텍스처의 크기를 조절합니다.")]
        public float tiling = 1.0f;
    }
}