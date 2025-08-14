using UnityEngine;
using VContainer;

namespace Ingame
{
    public class PlayerMoveable : PawnMoveable
    {
        [Inject] private readonly PlayerInputContext inputContext;
        private CharacterController characterController;
        [Inject] private readonly PlayerPhysics playerPhysics;

        protected PlayerModel playerModel;
        private PlayerInputConfig inputConfig => playerModel.inputConfig;

        private float jumpForce { get => pawnModel.pawnTotalStat.jumpForce * physicsSetting.JUMP_FORCE_CONSTANT; }
        private float moveSpeed { get => playerModel.pawnTotalStat.moveSpeed * physicsSetting.MOVE_SPEED_CONSTANT; }
        private float sprintSpeed { get => playerModel.pawnTotalStat.sprintSpeed * physicsSetting.MOVE_SPEED_CONSTANT; }
        private float nowSpeed { get => GetNowSpeed(); }

        private Vector3 moveDirection;

        public override void Initialize()
        {
            base.Initialize();
            playerModel = pawnModel as PlayerModel;
            characterController = transform.GetComponent<CharacterController>();
            if (characterController == null)
                characterController = transform.gameObject.AddComponent<CharacterController>();
        }

        public override void Update()
        {
            base.Update();
            UpdateInput();
            ApplyMovement();
        }

        private void UpdateInput()
        {
            float horizontal = inputContext.GetAxis("Horizontal");
            float vertical = inputContext.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            moveDirection = transform.TransformDirection(direction);
        }

        public override void ClearVelocity()
        {
            playerPhysics.ResetVelocity();
        }

        private void ApplyMovement()
        {
            playerPhysics.Update();

            Vector3 finalMove = moveDirection * nowSpeed;
            finalMove.y = playerPhysics.VerticalVelocity;

            characterController.Move(finalMove * Time.deltaTime);
        }

        public override void Jump()
        {
            if (!IsGrounded()) return;
            playerPhysics.SetVelocity(Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y));
        }

        public override bool IsGrounded()
        {
            return characterController.isGrounded;
        }

        public float GetNowSpeed()
        {
            if (!inputContext.GetKey(inputConfig.sprintKey))
                return moveSpeed;
            else
                return sprintSpeed;
        }
    }
}