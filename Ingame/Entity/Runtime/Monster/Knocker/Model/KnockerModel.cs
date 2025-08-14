using UnityEngine;
using System;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class KnockerModel : MonsterModel
    {
    #region ========== Event ==========

    #endregion ====================

    #region ========== Data ==========

    #endregion ====================

    #region ========== State ==========

    #endregion ====================

        public new KnockerModelData Data => base.Data as KnockerModelData;
        public KnockerModel(KnockerModelData data, KnockerModelState state = null) : base(data, state)
        {
        }
    }
}