using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Corelib.Utils;

namespace World
{
    public class DesignArea : MonoBehaviour
    {
        public List<PBoxArea> boxAreas = new();
        public List<PSphereArea> sphereAreas = new();

        public IEnumerable<PBox> Boxes => boxAreas != null ? boxAreas.Select(b => b.Box) : Enumerable.Empty<PBox>();
        public IEnumerable<PSphere> Spheres => sphereAreas != null ? sphereAreas.Select(s => s.Sphere) : Enumerable.Empty<PSphere>();
    }
}
