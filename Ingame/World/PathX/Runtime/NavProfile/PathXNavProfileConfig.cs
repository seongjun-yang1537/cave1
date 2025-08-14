using System;
using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;

namespace PathX
{
    [Serializable]
    public class PathXNavProfileConfig : IEquatable<PathXNavProfileConfig>
    {
        [SerializeField]
        public List<PTriangle> triangles = new();

        public int chunkSize = 2;

        public bool Equals(PathXNavProfileConfig other)
        {
            if (other == null) return false;
            if (chunkSize != other.chunkSize) return false;
            if (triangles == null && other.triangles == null) return true;
            if (triangles == null || other.triangles == null) return false;
            if (triangles.Count != other.triangles.Count) return false;

            for (int i = 0; i < triangles.Count; i++)
            {
                if (!triangles[i].Equals(other.triangles[i]))
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is PathXNavProfileConfig config && Equals(config);
        }

        public override int GetHashCode()
        {
            int hash = chunkSize;
            foreach (var tri in triangles)
                hash ^= tri.GetHashCode();
            return hash;
        }

        public PathXNavProfileConfig Clone()
        {
            return new PathXNavProfileConfig
            {
                triangles = new List<PTriangle>(this.triangles),
                chunkSize = this.chunkSize
            };
        }
    }
}
