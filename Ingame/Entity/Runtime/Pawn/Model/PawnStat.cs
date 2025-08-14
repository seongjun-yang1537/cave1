using System;

namespace Ingame
{
    [Serializable]
    public class PawnStat
    {
        public float moveSpeed;
        public float rotationSpeed;
        public float sprintSpeed;
        public float jumpForce;

        public PawnStat()
        {
        }

        public PawnStat(PawnStat source)
        {
            moveSpeed = source.moveSpeed;
            rotationSpeed = source.rotationSpeed;
            sprintSpeed = source.sprintSpeed;
            jumpForce = source.jumpForce;
        }
    }
}
