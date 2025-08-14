using UnityEngine;
using System;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class CockroachModel : MonsterModel
    {
        #region ========== Event ==========

        #endregion ====================

        #region ========== Data ==========

        #endregion ====================

        #region ========== State ==========

        #endregion ====================

        public new CockroachModelData Data => base.Data as CockroachModelData;

        public CockroachModel(CockroachModelData data, CockroachModelState state = null) : base(data, state)
        {
        }
    }
}