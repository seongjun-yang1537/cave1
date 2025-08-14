using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class OBJGatewayModelData : EntityModelData
    {
        public override Type TargetType => typeof(OBJGatewayModel);
    }
}

