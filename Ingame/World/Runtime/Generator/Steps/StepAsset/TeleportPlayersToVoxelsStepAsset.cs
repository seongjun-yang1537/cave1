using UnityEngine;
using PathX;

namespace World
{
    [System.Serializable]
    public class TeleportPlayersToVoxelsStepAsset : WorldPopulatePipelineStep
    {
        public bool teleportToStart = true;
        public bool teleportToEnd = true;
        public TriangleDomain domain = TriangleDomain.Ground0;

        public override IWorldgenStep CreateStep()
        {
            return new TeleportPlayersToVoxelsStep(teleportToStart, teleportToEnd, domain);
        }
    }
}
