using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    public class PlayerViewHandler
    {
        private readonly PlayerController playerController;
        private IInputSource defaultSource = new KeyboardInputSource();
        private readonly IInputSource nullSource = new NullInputSource();
        private IInputSource moveLockSource;
        private bool actionsEnabled = true;
        private bool moveViewEnabled = true;
        public bool viewLocked { get; private set; }
        public UnityEvent<bool> onViewLockChanged { get; } = new();

        public PlayerViewHandler(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        public void OnEnable()
        {
            UpdateInputState();
        }

        public void SetActionEnabled(bool enabled)
        {
            actionsEnabled = enabled;
            UpdateInputState();
        }

        public void SetMoveAndViewEnabled(bool enabled)
        {
            moveViewEnabled = enabled;
            UpdateInputState();
        }

        public void SetViewLocked(bool locked)
        {
            if (viewLocked == locked) return;
            viewLocked = locked;
            onViewLockChanged.Invoke(locked);
        }

        private void UpdateInputState()
        {
            var inputContext = playerController.inputContext;
            var inputConfig = playerController.playerModel.inputConfig;

            if (!actionsEnabled)
            {
                inputContext.SetSource(nullSource);
                SetViewLocked(true);
            }
            else if (!moveViewEnabled)
            {
                moveLockSource ??= new MoveLockInputSource(defaultSource, inputConfig.primaryItemKey, inputConfig.secondaryItemKey);
                inputContext.SetSource(moveLockSource);
                SetViewLocked(true);
            }
            else
            {
                inputContext.SetSource(defaultSource);
                SetViewLocked(false);
            }
        }

        private class NullInputSource : IInputSource
        {
            public bool GetKey(KeyCode keyCode) => false;
            public bool GetKeyDown(KeyCode keyCode) => false;
            public bool GetKeyUp(KeyCode keyCode) => false;
            public float GetAxis(string preset) => 0f;
        }

        private class MoveLockInputSource : IInputSource
        {
            private readonly IInputSource origin;
            private readonly KeyCode primary;
            private readonly KeyCode secondary;

            public MoveLockInputSource(IInputSource origin, KeyCode primary, KeyCode secondary)
            {
                this.origin = origin;
                this.primary = primary;
                this.secondary = secondary;
            }

            private bool BlockKey(KeyCode key) => key == primary || key == secondary;

            public bool GetKey(KeyCode keyCode)
            {
                if (BlockKey(keyCode)) return false;
                return origin.GetKey(keyCode);
            }

            public bool GetKeyDown(KeyCode keyCode)
            {
                if (BlockKey(keyCode)) return false;
                return origin.GetKeyDown(keyCode);
            }

            public bool GetKeyUp(KeyCode keyCode)
            {
                if (BlockKey(keyCode)) return false;
                return origin.GetKeyUp(keyCode);
            }

            public float GetAxis(string preset)
            {
                if (preset == "Horizontal" || preset == "Vertical") return 0f;
                return origin.GetAxis(preset);
            }
        }
    }
}
