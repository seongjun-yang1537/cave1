using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class NotMoveable : IMoveable
    {
        public void Update()
        {

        }

        public void FixedUpdate()
        {

        }

        public bool IsGrounded()
        {
            return false;
        }

        public void Jump()
        {

        }

        public void Move(Vector3 direction)
        {

        }

        public Vector3? GetNextPosition()
        {
            return null;
        }

        public Quaternion? GetNextRotation()
        {
            return null;
        }
    }
}