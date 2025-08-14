using UnityEngine;
using System;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class RuptureBlastModel : ProjectileModel
    {
    #region ========== Event ==========

    #endregion ====================

    #region ========== Data ==========

    #endregion ====================

    #region ========== State ==========

    #endregion ====================

        public new RuptureBlastModelData Data => base.Data as RuptureBlastModelData;
        public RuptureBlastModel(RuptureBlastModelData data, RuptureBlastModelState state = null) : base(data, state)
        {
        }
    }
}