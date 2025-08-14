using System;

namespace Ingame
{
    [Serializable]
    public class RuptureModel : MonsterModel
    {
    #region ========== Event ==========

    #endregion ====================

    #region ========== Data ==========

    #endregion ====================

    #region ========== State ==========

    #endregion ====================

        public float explosionRange = 10.0f;
        public float explosionFuseTime = 1.5f;

        public new RuptureModelData Data => base.Data as RuptureModelData;

        public RuptureModel(RuptureModelData data, RuptureModelState state = null) : base(data, state)
        {
            if (state != null)
            {
                explosionRange = state.explosionRange;
                explosionFuseTime = state.explosionFuseTime;
            }
        }
    }
}