using UnityEngine;

namespace PathX
{
    public class NavPortal
    {
        public Vector3 left, right;
        public NavPortal(Vector3 left, Vector3 right)
        {
            this.left = left;
            this.right = right;
        }
    }
}