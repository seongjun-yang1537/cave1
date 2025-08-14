using UnityEngine;

namespace Ingame
{
    public class KeyboardInputSource : IInputSource
    {
        public bool GetKey(KeyCode keyCode)
        {
            return Input.GetKey(keyCode);
        }

        public bool GetKeyDown(KeyCode keyCode)
        {
            return Input.GetKeyDown(keyCode);
        }

        public bool GetKeyUp(KeyCode keyCode)
        {
            return Input.GetKeyUp(keyCode);
        }

        public float GetAxis(string preset)
        {
            return Input.GetAxis(preset);
        }
    }
}