using UnityEngine;
using System;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class EnvironmentModel : EntityModel
    {
    #region ========== Event ==========

    #endregion ====================

    #region ========== Data ==========

    #endregion ====================

    #region ========== State ==========

    #endregion ====================

        public new EnvironmentModelData Data => base.Data as EnvironmentModelData;
        public EnvironmentModel(EnvironmentModelData data, EnvironmentModelState state = null) : base(data, state)
        {
        }
    }
}