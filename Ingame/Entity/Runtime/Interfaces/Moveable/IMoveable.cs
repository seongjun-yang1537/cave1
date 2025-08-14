using UnityEngine;

namespace Ingame
{
    public interface IMoveable
    {
        public void Update() { }

        public void FixedUpdate() { }

        public void ClearVelocity() { }

        public Vector3? GetNextPosition();
        public Quaternion? GetNextRotation();
        public bool IsGrounded();
        public void Jump();
    }
}