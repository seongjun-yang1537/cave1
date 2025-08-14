using UnityEngine;
using System;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class DragonBoarModel : MonsterModel
    {
    #region ========== Event ==========

    #endregion ====================

    #region ========== Data ==========

    #endregion ====================

    #region ========== State ==========

    #endregion ====================

        public float wanderRadius = 15f;

        public new DragonBoarModelData Data => base.Data as DragonBoarModelData;

        public DragonBoarModel(DragonBoarModelData data, DragonBoarModelState state = null) : base(data, state)
        {
            if (state != null)
            {
                wanderRadius = state.wanderRadius;
            }
        }
    }
}