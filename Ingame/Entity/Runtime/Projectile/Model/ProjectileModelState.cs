using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class ProjectileModelState : EntityModelState
    {
        public float damage;
        public float speed;
        public float lifeTime;
        public float range;
        public LayerMask targetLayer;

        [NonSerialized] public EntityModel owner;
        [NonSerialized] public Transform followTarget;
        [NonSerialized] public UnityAction onHitTarget;
        [NonSerialized] public bool isSpanwed;
        public override Type TargetType => typeof(ProjectileModel);
    }
}
