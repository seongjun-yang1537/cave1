using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class OBJGatewayModelState : EntityModelState
    {
        public override Type TargetType => typeof(OBJGatewayModel);
    }
}
