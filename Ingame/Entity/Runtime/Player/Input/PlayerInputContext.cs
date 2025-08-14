using UnityEngine;
using VContainer;

namespace Ingame
{
    public class PlayerInputContext
    {
        [Inject] private IInputSource inputSource;

        public void SetSource(IInputSource source) => inputSource = source;

        public bool GetKey(KeyCode keyCode) => inputSource?.GetKey(keyCode) ?? false;
        public bool GetKeyDown(KeyCode keyCode) => inputSource?.GetKeyDown(keyCode) ?? false;
        public bool GetKeyUp(KeyCode keyCode) => inputSource?.GetKeyUp(keyCode) ?? false;
        public float GetAxis(string preset) => inputSource?.GetAxis(preset) ?? 0f;
    }
}