using System;
using UnityEngine;
using Cinemachine;
using TriInspector;
using Corelib.Utils;
using UnityEngine.Events;

namespace Core
{
    [DeclareBoxGroup("ViewLock", Title = "View Lock")]
    [DeclareBoxGroup("Controls", Title = "Controls")]
    [DeclareBoxGroup("Runtime", Title = "Runtime")]
    public sealed class CinemachineControlSetting : PersistentSingleton<CinemachineControlSetting>
    {
        [Group("ViewLock")] public bool viewLocked;

        [Group("Controls"), Button("Lock")] private void BtnLock() => SetViewLocked(true);
        [Group("Controls"), Button("Unlock")] private void BtnUnlock() => SetViewLocked(false);
        [Group("Controls"), Button("Toggle")] private void BtnToggle() => SetViewLocked(!ViewLocked);

        [Group("Runtime"), ShowInInspector, ReadOnly]
        private string Status => ViewLocked ? "Locked" : "Unlocked";

        public static UnityEvent<bool> OnViewLockChanged;

        private static bool s_hooked;
        private static CinemachineCore.AxisInputDelegate s_original;

        public static bool ViewLocked => Instance != null && Instance.viewLocked;

        public static void SetViewLocked(bool locked)
        {
            if (!Instance) return;
            if (Instance.viewLocked == locked) return;
            Instance.viewLocked = locked;
            OnViewLockChanged?.Invoke(locked);
        }

        private void OnEnable()
        {
            if (!s_hooked)
            {
                s_original = CinemachineCore.GetInputAxis;
                CinemachineCore.GetInputAxis = AxisHook;
                s_hooked = true;
            }
        }

        private void OnDisable()
        {
            if (Instance == this)
            {
                if (s_hooked)
                {
                    CinemachineCore.GetInputAxis = s_original;
                    s_hooked = false;
                }
            }
        }

        private static float AxisHook(string axisName)
        {
            if (ViewLocked && (axisName == "Mouse X" || axisName == "Mouse Y"))
                return 0f;

            return s_original != null ? s_original(axisName) : Input.GetAxis(axisName);
        }
    }

}