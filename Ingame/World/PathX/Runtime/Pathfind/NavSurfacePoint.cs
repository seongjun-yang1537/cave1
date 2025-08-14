using UnityEngine;

namespace PathX
{
    public class NavSurfacePoint
    {
        public Vector3 point;
        public Vector3 normal;
        public NavSurfacePoint(Vector3 point, Vector3 normal)
        {
            this.point = point;
            this.normal = normal;
        }

        public void Deconstruct(out Vector3 point, out Vector3 normal)
        {
            point = this.point;
            normal = this.normal;
        }
    }
}