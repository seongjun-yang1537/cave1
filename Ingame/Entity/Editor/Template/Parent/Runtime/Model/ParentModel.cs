using UnityEngine;
using System;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class ParentModel : PawnModel
    {
    #region ========== Event ==========

    #endregion ====================

    #region ========== Data ==========

    #endregion ====================

    #region ========== State ==========

    #endregion ====================

        public new ParentModelData Data => base.Data as ParentModelData;

        public ParentModel(ParentModelData data, ParentModelState state = null) : base(data, state)
        {
        }
    }
}