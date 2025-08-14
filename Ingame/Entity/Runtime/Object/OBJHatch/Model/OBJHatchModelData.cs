using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class OBJHatchModelData : EntityModelData
    {
        public override Type TargetType => typeof(OBJHatchModel);
    }
}

