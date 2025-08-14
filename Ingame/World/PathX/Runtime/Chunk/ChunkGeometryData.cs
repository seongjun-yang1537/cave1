using System;
using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;

namespace PathX
{
    [Serializable]
    public class ChunkGeometryData<T> where T : PTriangle
    {
        public Vector3Int idx;
        public PCube chunkArea;
        public List<T> triangles;

        public bool Contains(T triangle)
        {
            if (this.chunkArea == null) return false;

            var triMin = Vector3.Min(triangle.v0, Vector3.Min(triangle.v1, triangle.v2));
            var triMax = Vector3.Max(triangle.v0, Vector3.Max(triangle.v1, triangle.v2));

            var chunkMin = this.chunkArea.Min;
            var chunkMax = this.chunkArea.Max;

            bool overlapX = triMin.x <= chunkMax.x && triMax.x >= chunkMin.x;
            bool overlapY = triMin.y <= chunkMax.y && triMax.y >= chunkMin.y;
            bool overlapZ = triMin.z <= chunkMax.z && triMax.z >= chunkMin.z;

            return overlapX && overlapY && overlapZ;
        }
    }
}