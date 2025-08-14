using System;

namespace World
{
    [Flags]
    public enum BiomeFlags
    {
        None = 0,
        Hollow = 1 << 0,
        Threadway = 1 << 1,
        Behemire = 1 << 2,
        Veinreach = 1 << 3,
        Fangspire = 1 << 4,
        Silken = 1 << 5,
        All = ~0
    }

    public static class BiomeFlagsExtensions
    {
        public static BiomeFlags ToFlag(this BiomeType type)
        {
            return type switch
            {
                BiomeType.Hollow => BiomeFlags.Hollow,
                BiomeType.Threadway => BiomeFlags.Threadway,
                BiomeType.Behemire => BiomeFlags.Behemire,
                BiomeType.Veinreach => BiomeFlags.Veinreach,
                BiomeType.Fangspire => BiomeFlags.Fangspire,
                BiomeType.Silken => BiomeFlags.Silken,
                _ => BiomeFlags.None
            };
        }
    }
}
