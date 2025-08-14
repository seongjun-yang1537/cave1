using UnityEngine;

namespace Ingame
{
    public class StaticMoveable : IMoveable
    {
        public Vector3? GetNextPosition() => null;

        public Quaternion? GetNextRotation() => null;

        public bool IsGrounded() => false;

        public void Jump() { }
    }
}