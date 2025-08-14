using UnityEngine;
using System;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class OBJGatewayModel : EntityModel
    {
    #region ========== Event ==========

    #endregion ====================

    #region ========== Data ==========

    #endregion ====================

    #region ========== State ==========

    #endregion ====================

        public new OBJGatewayModelData Data => base.Data as OBJGatewayModelData;
        public OBJGatewayModel(OBJGatewayModelData data, OBJGatewayModelState state = null) : base(data, state)
        {
        }
    }
}