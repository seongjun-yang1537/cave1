using UnityEngine;

namespace Ingame
{
    /// <summary>
    /// Handles vertical physics for the player such as gravity and jetpack force.
    /// This centralizes vertical velocity to avoid duplicated calculations
    /// between <see cref="PlayerMoveable"/> and <see cref="PlayerJetpackHandler"/>.
    /// </summary>
    public class PlayerPhysics
    {
        private readonly CharacterController characterController;

        /// <summary>Current vertical velocity used for CharacterController movement.</summary>
        public float VerticalVelocity { get; private set; }

        private const float FALL_DAMAGE_THRESHOLD = 10f;
        private const float FALL_DAMAGE_MULTIPLIER = 20f;

        private bool wasGrounded;
        private float maxFallSpeed;

        public bool IsGrounded => characterController.isGrounded;

        public PlayerPhysics(CharacterController characterController)
        {
            this.characterController = characterController;
            ResetFallState();
        }

        /// <summary>Apply gravity to the vertical velocity.</summary>
        public void Update()
        {
            if (characterController.isGrounded && VerticalVelocity < 0f)
                VerticalVelocity = -2f;

            VerticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        /// <summary>Adds a force to the vertical velocity.</summary>
        public void AddForce(float force)
        {
            VerticalVelocity += force;
        }

        /// <summary>Sets the vertical velocity directly.</summary>
        public void SetVelocity(float velocity)
        {
            VerticalVelocity = velocity;
        }

        /// <summary>Reset vertical velocity to zero.</summary>
        public void ResetVelocity()
        {
            VerticalVelocity = 0f;
        }

        /// <summary>
        /// Resets internal fall damage tracking.
        /// </summary>
        public void ResetFallState()
        {
            wasGrounded = IsGrounded;
            maxFallSpeed = 0f;
        }

        /// <summary>
        /// Calculates fall damage based on the current landing state.
        /// Should be called after character movement each frame.
        /// </summary>
        /// <returns>The damage that should be applied. Zero if no damage.</returns>
        public float ProcessFallDamage()
        {
            float damage = 0f;
            bool grounded = IsGrounded;

            if (grounded)
            {
                if (!wasGrounded)
                {
                    float fallSpeed = -maxFallSpeed;
                    if (fallSpeed > FALL_DAMAGE_THRESHOLD)
                    {
                        damage = (fallSpeed - FALL_DAMAGE_THRESHOLD) * FALL_DAMAGE_MULTIPLIER;
                    }
                }

                maxFallSpeed = 0f;
            }
            else
            {
                if (VerticalVelocity < maxFallSpeed)
                    maxFallSpeed = VerticalVelocity;
                else if (VerticalVelocity > 0f)
                    maxFallSpeed = 0f;
            }

            wasGrounded = grounded;
            return damage;
        }
    }
}
