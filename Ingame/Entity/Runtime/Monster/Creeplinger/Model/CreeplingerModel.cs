using UnityEngine;
using System;

namespace Ingame
{
    [Serializable]
    public class CreeplingerModel : MonsterModel
    {
        #region ========== Event ==========

        #endregion ====================

        #region ========== Data ==========

        #endregion ====================

        #region ========== State ==========

        #endregion ====================

        public new CreeplingerModelData Data => base.Data as CreeplingerModelData;

        public CreeplingerModel(CreeplingerModelData data, CreeplingerModelState state = null) : base(data, state)
        {
        }
    }
}