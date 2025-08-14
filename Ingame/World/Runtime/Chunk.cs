using UnityEngine;
using VoxelEngine;

namespace World
{
    public class Chunk
    {
        public Vector3Int coord;
        public BitpackedScalarField field;

        public Chunk(Vector3Int coord, BitpackedScalarField field)
        {
            this.coord = coord;
            this.field = field;
        }
    }
}
