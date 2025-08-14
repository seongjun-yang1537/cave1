using UnityEngine;
using System;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class OBJHatchModel : EntityModel
    {
    #region ========== Event ==========

    #endregion ====================

    #region ========== Data ==========

    #endregion ====================

    #region ========== State ==========

    #endregion ====================

        public new OBJHatchModelData Data => base.Data as OBJHatchModelData;
        public OBJHatchModel(OBJHatchModelData data, OBJHatchModelState state = null) : base(data, state)
        {
        }
    }
}