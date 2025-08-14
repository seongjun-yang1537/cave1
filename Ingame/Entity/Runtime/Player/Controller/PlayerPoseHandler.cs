using Corelib.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class PlayerPoseHandler : ILifecycleInjectable
    {
        private readonly PlayerController playerController;
        private readonly CharacterController characterController;

        private PlayerModel playerModel { get => playerController.playerModel; }
        private PawnPhysicsSetting physicsSetting { get => playerModel.physicsSetting; }
        private PlayerInputConfig inputConfig => playerModel.inputConfig;

        public PlayerPoseHandler(PlayerController playerController)
        {
            this.playerController = playerController;
            characterController = playerController.GetComponent<CharacterController>();
        }

        public void OnEnable()
        {
            playerModel.onPoseState.AddListener(OnPoseState);
        }

        public void Update(PlayerInputContext inputContext)
        {
            UpdatePoseState(inputContext);
        }

        public void OnDisable()
        {
            playerModel.onPoseState.RemoveListener(OnPoseState);

        }

        private void UpdatePoseState(PlayerInputContext inputContext)
        {
            PawnPoseState poseState = PawnPoseState.None;
            void SetFlag(PawnPoseState state, bool active)
            {
                if (active) poseState |= state;
                else poseState &= ~state;
            }

            bool isCrouch = inputContext.GetKey(inputConfig.crouchKey);
            bool isProne = inputContext.GetKey(inputConfig.proneKey);
            bool isStanding = !isCrouch && !isProne;

            SetFlag(PawnPoseState.Standing, isStanding);
            SetFlag(PawnPoseState.Crouch, isCrouch && !isProne);
            SetFlag(PawnPoseState.Prone, isProne);

            bool isWalk = !Mathf.Approximately(GetInputDirection(inputContext).sqrMagnitude, 0f);
            bool isRunning = inputContext.GetKey(inputConfig.sprintKey) && playerModel.stemina > 0.5f;

            SetFlag(PawnPoseState.Walk, isWalk && !isRunning);
            if (isProne || isCrouch)
                SetFlag(PawnPoseState.Walk, isWalk);
            SetFlag(PawnPoseState.Run, !isProne && !isCrouch && isWalk && isRunning);

            playerController.ChangePose(poseState);
        }

        private void OnPoseState(PawnPoseState poseState)
        {
            foreach (var (state, preset) in physicsSetting.poseColliderPreset.presets)
            {
                if (poseState.HasFlag(state))
                    ApplyPoseCollider(preset);
            }
        }

        private void ApplyPoseCollider(CapsuleColliderPreset preset)
        {
            characterController.center = preset.center;
            characterController.height = preset.height;
            characterController.radius = preset.radius;
        }

        private Vector3 GetInputDirection(PlayerInputContext inputContext)
        => new Vector3(inputContext.GetAxis("Horizontal"), 0f, inputContext.GetAxis("Vertical"))
            .normalized;
    }
}