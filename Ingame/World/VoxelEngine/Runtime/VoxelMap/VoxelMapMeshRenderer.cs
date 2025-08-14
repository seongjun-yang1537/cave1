using System.Collections.Generic;
using UnityEngine;

namespace VoxelEngine
{
    [ExecuteAlways]
    [RequireComponent(typeof(VoxelMap))]
    public class VoxelMapMeshRenderer : ChunkedScalarFieldMeshRenderer
    {
        protected VoxelMap voxelMap;

        protected override void OnEnable()
        {
            base.OnEnable();

            voxelMap = GetComponent<VoxelMap>();

            voxelMap.onRenderVoxelMap.AddListener(OnRenderVoxelMap);
            voxelMap.onRenderChunkVoxelMap.AddListener(OnRenderVoxelMapChunk);
            voxelMap.onVisible.AddListener(OnVisible);
        }

        protected virtual void OnDisable()
        {
            voxelMap.onRenderVoxelMap.RemoveListener(OnRenderVoxelMap);
            voxelMap.onRenderChunkVoxelMap.RemoveListener(OnRenderVoxelMapChunk);
            voxelMap.onVisible.RemoveListener(OnVisible);
        }

        protected virtual void OnVisible(bool visible)
        {
            this.Visible = visible;
        }

        protected virtual void OnRenderVoxelMap(ChunkedScalarField field)
        {
            RenderField(field);
        }

        protected virtual void OnRenderVoxelMapChunk(ChunkedScalarField field, IEnumerable<Vector3Int> chunkCoords)
        {
            RenderChunks(field, chunkCoords);
        }
    }
}