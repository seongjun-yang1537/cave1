using System;
using UnityEngine;

namespace PathX
{
    [Serializable]
    public class PathXEngineConfig
    {
        [SerializeField]
        public MeshFilter meshFilter;
        public int chunkSize = 2;

        public bool IsValid => meshFilter != null && meshFilter.sharedMesh != null;

        public Mesh Mesh => meshFilter?.sharedMesh;

        public bool Equals(PathXEngineConfig other)
        {
            if (other == null) return false;
            return meshFilter == other.meshFilter && chunkSize == other.chunkSize;
        }

        public PathXEngineConfig Clone()
        {
            return new PathXEngineConfig
            {
                meshFilter = this.meshFilter,
                chunkSize = this.chunkSize
            };
        }
    }
}