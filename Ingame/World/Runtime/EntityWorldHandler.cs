using Core;
using Corelib.Utils;
using UnityEngine;

namespace World
{
    public class EntityWorldHandler : IEntityWorldHandler
    {
        public void Dig(Vector3 position)
        {
            WorldSystem.RemoveVoxel(Vector3Int.RoundToInt(position));
        }

        public void Dig(PSphere sphere)
        {
            WorldSystem.CarveSphere(sphere);
        }

        public void Dig(PBox box)
        {
            WorldSystem.CarveBox(box);
        }
    }
}