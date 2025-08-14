using UnityEngine;
using System;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class NPCModel : PawnModel
    {
    #region ========== Event ==========

    #endregion ====================

    #region ========== Data ==========

    #endregion ====================

    #region ========== State ==========

    #endregion ====================

        public new NPCModelData Data => base.Data as NPCModelData;
        public NPCModel(NPCModelData data, NPCModelState state = null) : base(data, state)
        {
        }
    }
}