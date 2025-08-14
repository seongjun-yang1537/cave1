using UnityEngine;

namespace VoxelEngine
{
    public interface IScalarField
    {
        public int this[int x, int y, int z] { get; set; }
        Vector3Int Size { get; }

        public bool IsInBounds(int x, int y, int z);
        public void Clear();
        public int CalculateIndex(int x, int y, int z);
    }
}