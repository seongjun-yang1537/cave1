using Corelib.Utils;
using UnityEngine;

namespace VoxelEngine
{
    public class VoxelChunkMeshCreator : DefaultChunkMeshCreator
    {
        public override GameObject Create(string name = "")
        {
            GameObject go = base.Create(name);

            go.AddComponent<WorldBoundsController>();
            return go;
        }
    }
}