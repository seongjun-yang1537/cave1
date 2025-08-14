using UnityEngine;
using System;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class MonsterModel : PawnModel
    {
        #region ========== Event ==========
        public UnityEvent<Vector3> onTargetPosition { get => pathProgress.onTargetPosition; }

        #endregion ====================

        #region ========== Data ==========
        public new MonsterModelData Data => base.Data as MonsterModelData;
        public float sightRange => Data.sightRange;
        public float attackRange => Data.attackRange;
        public float attackCooldown => Data.attackCooldown;
        public float enemyDetectionRange => Data.enemyDetectionRange;

        public ItemDropTable dropTable => Data.dropTable;
        #endregion ====================

        #region ========== State ==========
        [SerializeField]
        public PathProgress pathProgress;
        #endregion ====================

        public void ClearPath()
        {
            pathProgress.Clear();
        }
        public MonsterModel(MonsterModelData data, MonsterModelState state = null) : base(data, state)
        {
            pathProgress = new PathProgress();
        }
    }
}