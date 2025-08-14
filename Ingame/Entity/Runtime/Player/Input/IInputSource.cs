using UnityEngine;

namespace Ingame
{
    public interface IInputSource
    {
        public bool GetKey(KeyCode keyCode);
        public bool GetKeyDown(KeyCode keyCode);
        public bool GetKeyUp(KeyCode keyCode);
        public float GetAxis(string preset);
    }
}