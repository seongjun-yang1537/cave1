using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class PawnModel : AgentModel
    {
        #region ========== Event ==========
        [NonSerialized]
        public readonly UnityEvent<PawnPoseState> onPoseState = new();
        #endregion ====================

        #region ========== Data ==========
        public new PawnModelData Data => base.Data as PawnModelData;
        [SerializeField]
        public PawnStat pawnBaseStat;
        public PawnTotalStat pawnTotalStat;

        public PawnPhysicsSetting physicsSetting => Data.physicsSetting;
        #endregion ====================

        #region ========== State ==========
        public float nowSpeed;

        [field: SerializeField]
        public PawnPoseState poseState { get; private set; } = PawnPoseState.Standing;
        #endregion ====================

        public PawnModel(PawnModelData data, PawnModelState state = null) : base(data, state)
        {
            pawnBaseStat = new PawnStat(data.pawnBaseStat);
            pawnTotalStat = new PawnTotalStat(this);

            nowSpeed = 0f;

            if (state != null)
            {
                nowSpeed = state.nowSpeed;
            }
        }

        public void SetPoseState(PawnPoseState poseState)
        {
            if (this.poseState == poseState) return;

            this.poseState = poseState;
            onPoseState.Invoke(this.poseState);
        }

        public void SetPoseState(PawnPoseState poseState, bool active)
        {
            if (this.poseState.HasFlag(poseState) == active) return;

            if (active) this.poseState |= poseState;
            else this.poseState &= ~poseState;

            onPoseState.Invoke(this.poseState);
        }
    }
}