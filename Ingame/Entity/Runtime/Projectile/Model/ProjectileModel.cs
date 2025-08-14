using UnityEngine;
using System;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class ProjectileModel : EntityModel
    {
        #region ========== Event ==========

        #endregion ====================

        #region ========== Data ==========

        #endregion ====================

        #region ========== State ==========

        #endregion ====================

        [NonSerialized]
        public readonly UnityEvent onHit = new();

        public float damage => Data.damage;
        public float speed => Data.speed;
        public float lifeTime => Data.lifeTime;
        public float range => Data.range;
        public LayerMask targetLayer => Data.targetLayer;
        public EntityModel owner;

        public new ProjectileModelData Data => base.Data as ProjectileModelData;

        [NonSerialized]
        public Transform followTarget;
        public UnityAction onHitTarget;

        public void Hit(EntityModel target)
        {
            onHit.Invoke();
            onHitTarget?.Invoke();
        }
        public ProjectileModel(ProjectileModelData data, ProjectileModelState state = null) : base(data, state)
        {
            if (state != null)
            {
                owner = state.owner;
                followTarget = state.followTarget;
                onHitTarget = state.onHitTarget;
                isSpanwed = state.isSpanwed;
            }
        }
    }
}