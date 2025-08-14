using Cinemachine;
using Core;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    public enum CameraPerspective
    {
        FirstPerson,
        ThridPerson,
    }
    public class CameraManager : Singleton<CameraManager>
    {
        public CameraPerspective perspectiveState;
        public KeyCode perspectiveToggleKey;

        public Camera mainCamera;
        public CinemachineVirtualCamera firstPersonVCamera;
        public CinemachineVirtualCamera thirdPersonVCamera;
        private PlayerController player;

        void OnDisable()
        {
            if (player != null)
                player.onViewLockChanged.RemoveListener(OnViewLockChanged);
        }

        void OnViewLockChanged(bool locked)
        {
            CinemachineControlSetting.SetViewLocked(locked);
        }

        public void Awake()
        {
            mainCamera = transform.Find("Main Camera")?.GetComponent<Camera>();
            firstPersonVCamera = transform.Find("FirstPerson")?.GetComponent<CinemachineVirtualCamera>();
            thirdPersonVCamera = transform.Find("ThirdPerson")?.GetComponent<CinemachineVirtualCamera>();

            UpdatePerspectiveState(CameraPerspective.FirstPerson);
        }

        public void Update()
        {
            var current = PlayerSystem.CurrentPlayer;
            if (current != player)
            {
                if (player != null)
                    player.onViewLockChanged.RemoveListener(OnViewLockChanged);
                player = current;
                if (player != null)
                {
                    player.onViewLockChanged.AddListener(OnViewLockChanged);
                    OnViewLockChanged(player.viewLocked);
                }
            }

            if (Input.GetKeyDown(perspectiveToggleKey))
                TogglePerspercitveState();
        }

        public void TogglePerspercitveState()
        {
            switch (perspectiveState)
            {
                case CameraPerspective.FirstPerson:
                    {
                        UpdatePerspectiveState(CameraPerspective.ThridPerson);
                    }
                    break;
                case CameraPerspective.ThridPerson:
                    {
                        UpdatePerspectiveState(CameraPerspective.FirstPerson);
                    }
                    break;
            }
        }

        public void UpdatePerspectiveState(CameraPerspective state)
        {
            switch (state)
            {
                case CameraPerspective.FirstPerson:
                    {
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = false;

                        firstPersonVCamera.Priority = 10;
                        thirdPersonVCamera.Priority = 9;

                        mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Player"));
                    }
                    break;
                case CameraPerspective.ThridPerson:
                    {
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;

                        firstPersonVCamera.Priority = 9;
                        thirdPersonVCamera.Priority = 10;

                        mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("Player");
                    }
                    break;
            }
            perspectiveState = state;
        }
    }
}