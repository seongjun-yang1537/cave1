using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class ProjectileModelData : EntityModelData
    {
        public override Type TargetType => typeof(ProjectileModel);
        public float damage;
        public float speed;
        public float lifeTime;
        public float range;
        public LayerMask targetLayer;
    }
}

