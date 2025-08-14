using UnityEngine;
using System;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class OBJScrapMetalModel : EntityModel
    {
    #region ========== Event ==========

    #endregion ====================

    #region ========== Data ==========

    #endregion ====================

    #region ========== State ==========

    #endregion ====================

        public new OBJScrapMetalModelData Data => base.Data as OBJScrapMetalModelData;
        public OBJScrapMetalModel(OBJScrapMetalModelData data, OBJScrapMetalModelState state = null) : base(data, state)
        {
        }
    }
}