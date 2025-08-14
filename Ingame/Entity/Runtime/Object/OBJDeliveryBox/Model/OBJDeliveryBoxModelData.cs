using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class OBJDeliveryBoxModelData : EntityModelData
    {
        public override Type TargetType => typeof(OBJDeliveryBoxModel);
    }
}

