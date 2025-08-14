using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Ingame
{
    [RequireComponent(typeof(ProjectileScope))]
    public class ProjectileController : EntityController
    {
        public ProjectileModel projectileModel { get; private set; }
        public ProjectileView projectileView;

        [Inject] protected readonly IMoveable moveable;
        [Inject] private readonly IProjectileHitHandler hitHandler;

        public float currentLifeTime;

        protected override void Awake()
        {
            base.Awake();
            projectileModel = (ProjectileModel)entityModel;
            projectileView = (ProjectileView)entityView;
        }

        protected override void Start()
        {
            currentLifeTime = projectileModel.lifeTime;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            projectileModel.onHit.AddListener(OnHit);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            projectileModel.onHit.RemoveListener(OnHit);
        }

        protected override void Update()
        {
            if (!projectileModel.isSpanwed) return;
            HandleLifetime();
            HandleMovement();
            UpdateFollowTarget();
        }

        private void HandleLifetime()
        {
            if (currentLifeTime <= 0) return;

            currentLifeTime -= Time.deltaTime;
            if (currentLifeTime <= 0)
            {
                Dead();
            }
        }

        private void HandleMovement()
        {
            if (projectileModel.followTarget != null)
            {
                Vector3 direction = (projectileModel.followTarget.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction);
            }

            transform.position += transform.forward * projectileModel.speed * Time.deltaTime;
        }

        private void UpdateFollowTarget()
        {
            if (projectileModel.followTarget == null) return;

            float dist = 0.1f;
            float sqrDist = (projectileModel.followTarget.position - transform.position).sqrMagnitude;

            if (sqrDist < dist * dist)
            {
                Hit(projectileModel.followTarget.GetComponent<EntityController>());
            }
        }

        public List<EntityController> FindTargetsInRange()
        {
            var foundTargets = new List<EntityController>();
            if (projectileModel.range <= 0) return foundTargets;

            Collider[] colliders = Physics.OverlapSphere(transform.position, projectileModel.range, projectileModel.targetLayer);

            foreach (var col in colliders)
            {
                if (col.TryGetComponent<EntityController>(out var entity))
                {
                    if (entity.entityModel == projectileModel.owner)
                    {
                        continue;
                    }
                    foundTargets.Add(entity);
                }
            }
            return foundTargets;
        }

        public virtual void Hit(EntityController entityController)
        {
            projectileModel.Hit(entityController.entityModel);
            hitHandler?.Hit(this, entityController);
        }

        private void OnHit()
        {
            Dead();
        }
    }
}