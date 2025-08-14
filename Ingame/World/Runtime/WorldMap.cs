using System.Collections.Generic;
using Ingame;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using VoxelEngine;

namespace World
{
    public class WorldMap : VoxelMap
    {
        public UnityEvent<Vector3Int, bool> onChunkActive = new();

        public UnityEvent<ChunkedScalarField, WorldVSPGraph> onRenderWorldMap = new();

        public async Cysharp.Threading.Tasks.UniTask WaitForRenderCompleteAsync()
        {
            var renderer = GetComponent<VoxelEngine.ChunkedScalarFieldMeshRenderer>();
            if (renderer == null || !renderer.IsRendering)
                return;

            var tcs = new Cysharp.Threading.Tasks.UniTaskCompletionSource();
            UnityAction handler = null;
            handler = () =>
            {
                onRenderCompleteChunkMesh.RemoveListener(handler);
                tcs.TrySetResult();
            };
            onRenderCompleteChunkMesh.AddListener(handler);
            await tcs.Task;
        }

        public void SetWorldMap(ChunkedScalarField newScalarField, WorldVSPGraph graph)
        {
            RenderWorldMap(newScalarField, graph);
        }

        public void RenderWorldMap(ChunkedScalarField field, WorldVSPGraph graph)
        {
            onRenderWorldMap.Invoke(field, graph);
        }

        public void SetChunkActive(Vector3Int chunkCoord, bool active)
        {
            onChunkActive.Invoke(chunkCoord, active);
        }
    }
}