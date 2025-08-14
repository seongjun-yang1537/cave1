using Cysharp.Threading.Tasks;
using Corelib.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelEngine;

namespace World
{
    public class BuildScalarFieldFromGraphStep : IWorldgenRasterizeStep
    {
        private readonly int corridorSize;

        public BuildScalarFieldFromGraphStep(int corridorSize = 3)
        {
            this.corridorSize = Mathf.Max(1, corridorSize);
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            if (worldData == null) return UniTask.CompletedTask;

            if (worldData.chunkedScalarField == null ||
                worldData.chunkedScalarField.Size != worldData.treeSize)
                worldData.chunkedScalarField = new ChunkedScalarField(worldData.treeSize);

            var field = worldData.chunkedScalarField;

            foreach (var coord in field.GetLoadedChunkCoordinates())
            {
                var chunk = field.GetChunk(coord);
                var size = chunk.Size;
                for (int x = 0; x < size.x; x++)
                    for (int y = 0; y < size.y; y++)
                        for (int z = 0; z < size.z; z++)
                            chunk[x, y, z] = 1;
            }

            // if (VoxelMap != null)
            // {
            //     if (VoxelMap.fieldConfig == null)
            //         VoxelMap.fieldConfig = new ChunkedScalarFieldConfig();

            //     VoxelMap.fieldConfig.template = field;
            // }

            return UniTask.CompletedTask;
        }

    }
}
