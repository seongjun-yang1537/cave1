using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    public class PlayerOnlyAimTargetProvider : IAimTargetProvider
    {
        public readonly PlayerController playerController;

        public PlayerOnlyAimTargetProvider()
        {
            playerController = Object.FindAnyObjectByType<PlayerController>();
        }

        public int GetAimtarget()
        {
            return playerController?.entityModel?.entityID ?? -1;
        }
    }
}