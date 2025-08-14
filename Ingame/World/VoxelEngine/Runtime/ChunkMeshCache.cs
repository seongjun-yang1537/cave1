using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelEngine
{
    public class ChunkMeshCache
    {
        private readonly ChunkedScalarField field;
        private readonly Dictionary<Vector3Int, Mesh> meshCache;

        private const int PaddedSize = ChunkedScalarField.CHUNK_SIZE + 2;

        public ChunkMeshCache(ChunkedScalarField field)
        {
            this.field = field;
            this.meshCache = new Dictionary<Vector3Int, Mesh>();
        }

        public Mesh GetOrCreateMesh(Vector3Int chunkCoord, float isolevel)
        {
            if (meshCache.TryGetValue(chunkCoord, out Mesh mesh))
            {
                return mesh;
            }

            var paddedField = new BitpackedScalarField(PaddedSize, PaddedSize, PaddedSize);
            PopulatePaddedField(paddedField, chunkCoord);

            Mesh newMesh = MarchingCubesMesher.Generate(paddedField, isolevel);

            Vector3[] vertices = newMesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] -= Vector3.one;
            }
            newMesh.vertices = vertices;

            meshCache[chunkCoord] = newMesh;
            return newMesh;
        }

        private void PopulatePaddedField(BitpackedScalarField paddedField, Vector3Int chunkCoord)
        {
            Vector3Int globalOrigin = chunkCoord * ChunkedScalarField.CHUNK_SIZE;

            for (int x = 0; x < PaddedSize; x++)
                for (int y = 0; y < PaddedSize; y++)
                    for (int z = 0; z < PaddedSize; z++)
                    {
                        Vector3Int globalPos = globalOrigin + new Vector3Int(x - 1, y - 1, z - 1);

                        if (field.IsInBounds(globalPos.x, globalPos.y, globalPos.z))
                        {
                            paddedField[x, y, z] = field[globalPos.x, globalPos.y, globalPos.z];
                        }
                        else
                        {
                            paddedField[x, y, z] = 0;
                        }
                    }
        }

        public void InvalidateAll()
        {
            foreach (var mesh in meshCache.Values)
            {
                if (mesh != null) UnityEngine.Object.Destroy(mesh);
            }
            meshCache.Clear();
        }
    }
}