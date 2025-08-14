using UnityEngine;
using System;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class ToxicSpitModel : ProjectileModel
    {
        #region ========== Event ==========

        #endregion ====================

        #region ========== Data ==========

        #endregion ====================

        #region ========== State ==========

        #endregion ====================

        public new ToxicSpitModelData Data => base.Data as ToxicSpitModelData;
        public ToxicSpitModel(ToxicSpitModelData data, ToxicSpitModelState state = null) : base(data, state)
        {
        }
    }
}
