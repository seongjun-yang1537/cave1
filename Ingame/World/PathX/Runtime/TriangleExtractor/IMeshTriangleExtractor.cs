using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;

namespace PathX
{
    public interface IMeshTriangleExtractor
    {
        List<PTriangle> Extract(Mesh mesh);
    }
}