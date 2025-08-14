using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    public class ProjectileContext
    {
        public EntityType Type { get; }
        public float Damage { get; }
        public float Speed { get; }
        public float LifeTime { get; }
        public LayerMask TargetLayer { get; }
        public EntityModel Owner { get; }
        public Transform FollowTarget { get; }
        public UnityAction OnHitTarget { get; }
        public IProjectileHitHandler HitHandler { get; }

        private ProjectileContext(EntityType type,
        float damage,
        float speed,
        float lifeTime,
        LayerMask targetLayer,
        EntityModel owner,
        Transform followTarget,
        UnityAction onHitTarget,
        IProjectileHitHandler hitHandler)
        {
            Type = type;
            Damage = damage;
            Speed = speed;
            LifeTime = lifeTime;
            TargetLayer = targetLayer;
            Owner = owner;
            FollowTarget = followTarget;
            OnHitTarget = onHitTarget;
            HitHandler = hitHandler;
        }

        public class Builder
        {
            private readonly EntityType type;
            private float damage = 0f;
            private float speed = 0f;
            private float lifeTime = float.MaxValue;
            private LayerMask targetLayer;
            private EntityModel owner;
            private Transform followTarget;
            private UnityAction onHitTarget;
            private IProjectileHitHandler hitHandler = new DamageProjectileHitHandler();


            public Builder(EntityType type)
            {
                this.type = type;
            }

            public Builder HitHandler(IProjectileHitHandler hitHandler)
            {
                this.hitHandler = hitHandler;
                return this;
            }

            public Builder OnHitTarget(UnityAction onHitTarget)
            {
                this.onHitTarget = onHitTarget;
                return this;
            }


            public Builder SetDamage(float damage)
            {
                this.damage = damage;
                return this;
            }

            public Builder SetSpeed(float speed)
            {
                this.speed = speed;
                return this;
            }

            public Builder SetLifeTime(float lifeTime)
            {
                this.lifeTime = lifeTime;
                return this;
            }

            public Builder SetTargetLayer(LayerMask layer)
            {
                this.targetLayer = layer;
                return this;
            }

            public Builder SetOwner(EntityModel owner)
            {
                this.owner = owner;
                return this;
            }

            public Builder SetFollowTarget(Transform target)
            {
                this.followTarget = target;
                return this;
            }

            public ProjectileContext Build()
            {
                return new ProjectileContext(
                    type,
                    damage,
                    speed,
                    lifeTime,
                    targetLayer,
                    owner,
                    followTarget,
                    onHitTarget,
                    hitHandler
                );
            }
        }
    }
}