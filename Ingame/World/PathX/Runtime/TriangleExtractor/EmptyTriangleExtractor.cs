using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;

namespace PathX
{
    public class EmptyTriangleExtractor : IMeshTriangleExtractor
    {
        public List<PTriangle> Extract(Mesh mesh)
        {
            return new();
        }
    }
}