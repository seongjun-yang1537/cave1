using UnityEngine;
using System;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class MerchantNPCModel : NPCModel
    {
    #region ========== Event ==========

    #endregion ====================

    #region ========== Data ==========

    #endregion ====================

    #region ========== State ==========

    #endregion ====================

        public new MerchantNPCModelData Data => base.Data as MerchantNPCModelData;

        public MerchantNPCModel(MerchantNPCModelData data, MerchantNPCModelState state = null) : base(data, state)
        {
        }
    }
}