using System.Collections;
using Corelib.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Ingame
{
    public class PlayerView : PawnView
    {
        public UnityEvent<PlayerController, EntityController> onInteractTargetEnter = new();
        public UnityEvent<PlayerController, EntityController> onInteractTargetExit = new();

        private Camera mainCamera;
        private CharacterController characterController;

        protected override void Awake()
        {
            base.Awake();
            mainCamera = Camera.main;

            characterController = GetComponent<CharacterController>();
        }

        [AutoSubscribe(nameof(onDead))]
        protected virtual void OnDead(AgentController agentController, AgentModel agentModel)
        {
            StartCoroutine(LoadSceneCoroutine());
        }

        private IEnumerator LoadSceneCoroutine()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Outgame");

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        [AutoSubscribe(nameof(onWorldItemChanged))]
        protected override void OnWorldItemChanged(AgentController agentController, ItemModel itemModel, WorldItemMode mode)
        {
            if (mode == WorldItemMode.Drop)
            {
                DropItemByForward(itemModel, mainCamera.transform.forward);
            }
        }

        [AutoSubscribe(nameof(onPoseState))]
        protected virtual void OnPoseState(PawnController pawnController, PawnPoseState poseState)
        {
            string animPoseState = "";
            if (poseState.HasFlag(PawnPoseState.Standing))
                animPoseState = "Standing";
            if (poseState.HasFlag(PawnPoseState.Crouch))
                animPoseState = "Crouch";
            if (poseState.HasFlag(PawnPoseState.Prone))
                animPoseState = "Prone";

            string animPoseStateArg = "";
            if (!string.IsNullOrEmpty(animPoseState))
            {
                if (poseState.HasFlag(PawnPoseState.Walk))
                    animPoseStateArg = "@Walk";
                if (poseState.HasFlag(PawnPoseState.Run))
                    animPoseStateArg = "@Run";

            }
            PlayAnimation($"{animPoseState}{animPoseStateArg}");
        }

        [AutoSubscribe(nameof(onPosition))]
        protected override void OnPosition(EntityController entityController, Vector3 position)
        {
            PlayerController playerController = entityController as PlayerController;
            playerController.SetPosition(position);
        }
    }
}