using UnityEngine;
using System;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class AntModel : MonsterModel
    {
        #region ========== Event ==========

        #endregion ====================

        #region ========== Data ==========

        #endregion ====================

        #region ========== State ==========

        #endregion ====================

        public new AntModelData Data => base.Data as AntModelData;

        public AntModel(AntModelData data, AntModelState state = null) : base(data, state)
        {
        }
    }
}