using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;

namespace PathX
{
    public class AllTriangleExtractor : IMeshTriangleExtractor
    {
        public List<PTriangle> Extract(Mesh mesh)
        {
            return TriangleExtractor.ExtractAll(mesh);
        }
    }
}