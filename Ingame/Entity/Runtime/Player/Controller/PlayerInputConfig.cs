using System;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [CreateAssetMenu(fileName = "New Player Input Config", menuName = "Game/Config/Player Input")]
    public class PlayerInputConfig : ScriptableObject
    {
        public KeyCode sprintKey = KeyCode.LeftControl;
        public KeyCode jumpKey = KeyCode.Space;

        public KeyCode crouchKey = KeyCode.LeftShift;
        public KeyCode proneKey = KeyCode.Z;

        public KeyCode primaryItemKey = KeyCode.Mouse0;
        public KeyCode secondaryItemKey = KeyCode.Mouse1;

        public KeyCode interactKey = KeyCode.F;
    }
}