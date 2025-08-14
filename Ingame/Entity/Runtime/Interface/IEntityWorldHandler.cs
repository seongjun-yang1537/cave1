using Corelib.Utils;
using UnityEngine;

namespace Core
{
    public interface IEntityWorldHandler
    {
        public void Dig(Vector3 position);
        public void Dig(PSphere sphere);
        public void Dig(PBox box);
    }
}