using System;
using System.Collections.Generic;
using Corelib.Utils;
using PathX;
using UnityEngine;
using UnityEngine.Events;

namespace VoxelEngine
{
    [ExecuteAlways]
    public class VoxelMap : MonoBehaviour
    {
        public UnityEvent<ChunkedScalarField> onRenderVoxelMap = new();
        public UnityEvent<ChunkedScalarField, IEnumerable<Vector3Int>> onRenderChunkVoxelMap = new();
        public UnityEvent onRenderCompleteChunkMesh = new();
        public UnityEvent<bool> onVisible = new();

        [SerializeField]
        private bool meshVisible;
        public bool MeshVisible
        {
            get => meshVisible;
            set { meshVisible = value; ; onVisible.Invoke(value); }
        }

        public void SetVoxelMap(ChunkedScalarField newScalarField)
        {
            RenderVoxelMap(newScalarField);
        }

        public void SetVoxelMapDirtyChunk(ChunkedScalarField newField, IEnumerable<Vector3Int> chunkCoords)
        {
            RenderChunksVoxelMap(newField, chunkCoords);
        }

        public void SetVoxel(ChunkedScalarField field, Vector3Int position, int value)
        {
            if (field == null || !field.IsInBounds(position.x, position.y, position.z)) return;

            field[position.x, position.y, position.z] = value;

            var (chunkCoord, _) = field.GetChunkAndLocalCoord(position.x, position.y, position.z);
            SetVoxelMapDirtyChunk(field, new[] { chunkCoord });
        }

        public void AddVoxel(ChunkedScalarField field, Vector3Int position)
            => SetVoxel(field, position, 1);
        public void RemoveVoxel(ChunkedScalarField field, Vector3Int position)
            => SetVoxel(field, position, 0);

        public void SetBox(ChunkedScalarField field, PBox box, int value)
        {
            if (field == null) return;

            var min = Vector3Int.Max(box.Min.FloorToInt(), Vector3Int.zero);
            var max = Vector3Int.Min(box.Max.FloorToInt(), field.Size - Vector3Int.one);

            var modifiedChunks = new HashSet<Vector3Int>();

            for (int x = min.x; x <= max.x; x++)
            {
                for (int y = min.y; y <= max.y; y++)
                {
                    for (int z = min.z; z <= max.z; z++)
                    {
                        field[x, y, z] = value;
                        var (chunkCoord, _) = field.GetChunkAndLocalCoord(x, y, z);
                        modifiedChunks.Add(chunkCoord);
                    }
                }
            }

            SetVoxelMapDirtyChunk(field, modifiedChunks);
        }

        public void FillBox(ChunkedScalarField field, PBox box)
            => SetBox(field, box, 1);

        public void CarveBox(ChunkedScalarField field, PBox box)
            => SetBox(field, box, 0);

        public void SetSphere(ChunkedScalarField field, PSphere sphere, int value)
        {
            if (field == null) return;

            var min = Vector3Int.Max(sphere.Min, Vector3Int.zero);
            var max = Vector3Int.Min(sphere.Max, field.Size - Vector3Int.one);

            var modifiedChunks = new HashSet<Vector3Int>();

            for (int x = min.x; x <= max.x; x++)
            {
                for (int y = min.y; y <= max.y; y++)
                {
                    for (int z = min.z; z <= max.z; z++)
                    {
                        if (sphere.Contains(new Vector3Int(x, y, z)))
                        {
                            field[x, y, z] = value;
                            var (chunkCoord, _) = field.GetChunkAndLocalCoord(x, y, z);
                            modifiedChunks.Add(chunkCoord);
                        }
                    }
                }
            }

            SetVoxelMapDirtyChunk(field, modifiedChunks);
        }

        public void FillSphere(ChunkedScalarField field, PSphere sphere)
            => SetSphere(field, sphere, 1);

        public void CarveSphere(ChunkedScalarField field, PSphere sphere)
            => SetSphere(field, sphere, 0);

        public void RenderVoxelMap(ChunkedScalarField field)
        {
            onRenderVoxelMap.Invoke(field);
        }

        public void RenderChunksVoxelMap(ChunkedScalarField newField, IEnumerable<Vector3Int> chunkCoords)
        {
            onRenderChunkVoxelMap.Invoke(newField, chunkCoords);
        }
    }
}