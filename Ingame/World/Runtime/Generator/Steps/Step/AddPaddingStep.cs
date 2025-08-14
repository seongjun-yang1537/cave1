using Cysharp.Threading.Tasks;
using Corelib.Utils;
using UnityEngine;
using VoxelEngine;

namespace World
{
    public class AddPaddingStep : IWorldgenRasterizeStep
    {
        private readonly int padding;

        public AddPaddingStep(int padding)
        {
            this.padding = Mathf.Max(0, padding);
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            var field = worldData?.chunkedScalarField;
            if (field == null || padding <= 0) return UniTask.CompletedTask;

            Vector3Int oldSize = field.Size;
            Vector3Int newSize = oldSize + Vector3Int.one * padding * 2;
            var newField = new ChunkedScalarField(newSize);

            foreach (var pos in ExVector3Int.Spread(Vector3Int.zero, newSize - Vector3Int.one))
                newField[pos.x, pos.y, pos.z] = 1;

            for (int x = 0; x < oldSize.x; x++)
                for (int y = 0; y < oldSize.y; y++)
                    for (int z = 0; z < oldSize.z; z++)
                        newField[x + padding, y + padding, z + padding] = field[x, y, z];

            worldData.chunkedScalarField = newField;
            worldData.treeSize = newSize;
            worldData.padding += padding;
            return UniTask.CompletedTask;
        }
    }
}
