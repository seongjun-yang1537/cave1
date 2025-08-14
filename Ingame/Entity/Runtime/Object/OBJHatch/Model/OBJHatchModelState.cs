using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class OBJHatchModelState : EntityModelState
    {
        public override Type TargetType => typeof(OBJHatchModel);
    }
}
