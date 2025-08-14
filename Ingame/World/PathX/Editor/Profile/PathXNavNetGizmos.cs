using Corelib.SUI;
using Corelib.Utils;
using UnityEngine;

namespace PathX
{
    public static class PathXNavMeshGizmos
    {
        public static void Render(PathXNavMesh profile)
        {
            if (profile == null) return;

            RenderChunkIndex(profile.ChunkIndex);
        }

        private static void RenderChunkIndex<T>(ChunkGeometryIndex<T> chunkIndex) where T : PTriangle
        {
            if (chunkIndex == null) return;
            SGizmos.Scope(
                SGizmos.Action(() =>
                {
                    foreach (var chunk in chunkIndex.chunks.Values)
                    {
                        PCube area = chunk.chunkArea;
                        SGizmos.WireCube(area.center, area.size)
                        .Render();
                    }
                })
            )
            .Color(Color.red)
            .Render();
        }
    }
}