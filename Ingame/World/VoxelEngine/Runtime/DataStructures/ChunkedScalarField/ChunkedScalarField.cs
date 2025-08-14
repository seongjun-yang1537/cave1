using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelEngine
{
    [Serializable]
    public class ChunkedScalarField : IScalarField, ISerializationCallbackReceiver, IEnumerable<int>
    {
        public const int CHUNK_SIZE = 16;
        private const int CHUNK_SIZE_LOG2 = 4;
        private const int CHUNK_MASK = 15;

        [SerializeField]
        private Vector3Int fieldSize;

        [Serializable]
        private class ChunkDictionary : SerializableDictionary<Vector3Int, BitpackedScalarField> { }
        [SerializeField]
        private ChunkDictionary chunks;

        [SerializeField]
        private Vector3Int chunkCounts;

        public Vector3Int Size => fieldSize;

        public ChunkedScalarField(Vector3Int fieldSize)
        {
            Initialize(fieldSize);
        }

        public bool IsInBounds(int x, int y, int z) =>
            (uint)x < fieldSize.x && (uint)y < fieldSize.y && (uint)z < fieldSize.z;

        public int this[int x, int y, int z]
        {
            get
            {
                if (!IsInBounds(x, y, z))
                    throw new IndexOutOfRangeException($"({x}, {y}, {z}) is out of bounds.");

                var (chunkCoord, localCoord) = GetChunkAndLocalCoord(x, y, z);
                return chunks[chunkCoord][localCoord.x, localCoord.y, localCoord.z];
            }
            set
            {
                if (!IsInBounds(x, y, z))
                    throw new IndexOutOfRangeException($"({x}, {y}, {z}) is out of bounds.");

                var (chunkCoord, localCoord) = GetChunkAndLocalCoord(x, y, z);
                chunks[chunkCoord][localCoord.x, localCoord.y, localCoord.z] = value;
            }
        }

        public (Vector3Int chunkCoord, Vector3Int localCoord) GetChunkAndLocalCoord(int x, int y, int z)
        {
            var chunkCoord = new Vector3Int(
                x >> CHUNK_SIZE_LOG2,
                y >> CHUNK_SIZE_LOG2,
                z >> CHUNK_SIZE_LOG2
            );

            var localCoord = new Vector3Int(
                x & CHUNK_MASK,
                y & CHUNK_MASK,
                z & CHUNK_MASK
            );

            return (chunkCoord, localCoord);
        }

        public BitpackedScalarField GetChunk(Vector3Int chunkCoord)
        {
            chunks.TryGetValue(chunkCoord, out BitpackedScalarField chunk);
            return chunk;
        }

        public ICollection<Vector3Int> GetLoadedChunkCoordinates()
        {
            return chunks.Keys;
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            if (fieldSize.x <= 0) return;

            var expectedChunkCounts = new Vector3Int(
                (fieldSize.x + CHUNK_MASK) / CHUNK_SIZE,
                (fieldSize.y + CHUNK_MASK) / CHUNK_SIZE,
                (fieldSize.z + CHUNK_MASK) / CHUNK_SIZE
            );

            if (chunks == null || chunkCounts != expectedChunkCounts)
            {
                Initialize(fieldSize);
            }
        }

        private void Initialize(Vector3Int newFieldSize)
        {
            if (newFieldSize.x <= 0 || newFieldSize.y <= 0 || newFieldSize.z <= 0)
                throw new ArgumentOutOfRangeException(nameof(newFieldSize), "Field size must be positive.");

            this.fieldSize = newFieldSize;

            chunkCounts = new Vector3Int(
                (fieldSize.x + CHUNK_MASK) / CHUNK_SIZE,
                (fieldSize.y + CHUNK_MASK) / CHUNK_SIZE,
                (fieldSize.z + CHUNK_MASK) / CHUNK_SIZE
            );

            chunks = new();

            for (int cx = 0; cx < chunkCounts.x; cx++)
                for (int cy = 0; cy < chunkCounts.y; cy++)
                    for (int cz = 0; cz < chunkCounts.z; cz++)
                    {
                        var chunkCoord = new Vector3Int(cx, cy, cz);
                        chunks[chunkCoord] = new BitpackedScalarField(CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE);
                    }
        }

        public void Clear()
        {
            foreach (var chunk in chunks.Values)
                chunk.Clear();
        }

        public int CalculateIndex(int x, int y, int z)
        {
            if (!IsInBounds(x, y, z))
            {
                throw new IndexOutOfRangeException($"Coordinates ({x},{y},{z}) are out of the field's bounds.");
            }

            return z + y * Size.z + x * Size.z * Size.y;
        }

        public int CalculateIndex(Vector3Int idx)
            => CalculateIndex(idx.x, idx.y, idx.z);

        public IEnumerator<int> GetEnumerator()
        {
            for (int x = 0; x < fieldSize.x; x++)
            {
                for (int y = 0; y < fieldSize.y; y++)
                {
                    for (int z = 0; z < fieldSize.z; z++)
                    {
                        yield return this[x, y, z];
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}