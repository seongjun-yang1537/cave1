using System.Net.NetworkInformation;
using Core;
using Corelib.Utils;
using NUnit.Framework;
using PathX;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace Ingame
{
    [RequireComponent(typeof(EntityScope))]
    public class EntityController : ControllerBaseBehaviour
    {
        public bool applyRandomRotation = false;

        [Inject] public EntityModel entityModel;
        [Inject] public EntityView entityView;
        [Inject] public IEntityInteractable interactable;

        public bool isNotSnapToNavMesh;

        protected ISpawnable spawnable = new EntitySpawnable();
        protected PathXNavMesh NavMesh { get => PathXSystem.GetNavMesh(entityModel.navDomain); }

        #region LifeCycle

        protected override void OnEnable()
        {
            base.OnEnable();

            entityModel.onDead.AddListener(OnDead);

            if (!isNotSnapToNavMesh)
                SnapToNavMesh();
        }

        protected override void Start()
        {
            base.Start();
            EntitySystem.Register(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            EntitySystem.Unregister(this);

            entityModel.onDead.RemoveListener(OnDead);
        }
        #endregion

        #region Action
        public virtual void Interact(PlayerController playerController)
        {
            interactable?.Interact(this, playerController);
        }
        public virtual bool IsIntertable()
        {
            return interactable != null && interactable is not NoneEntityInteractable;
        }

        public virtual void Spawn()
        {
            spawnable?.Spawn(entityModel);
        }

        public virtual void MoveTo(Vector3 newPosition)
        {
            OnPosition(newPosition);
        }

        public virtual void RotateTo(Quaternion newRotation)
        {
            OnRotation(newRotation);
        }

        public NavSurfacePoint GetProjectedNavMeshSurfce()
        {
            if (NavMesh == null) return new NavSurfacePoint(transform.position, Vector3.up);
            Vector3 position = NavMesh.PointLocation(transform.position);
            Vector3 normal = (position == transform.position) ? Vector3.zero : (transform.position - position).normalized;
            return new NavSurfacePoint(position, normal);
        }

        public virtual void SnapToNavMesh()
        {
            NavSurfacePoint surfacePoint = GetProjectedNavMeshSurfce();
            MoveTo(surfacePoint.point);

            Quaternion randomYRotation = Quaternion.identity;
            if (applyRandomRotation)
            {
                float randomAngle = GameRng.Game.NextFloat(0f, 360f);
                randomYRotation = Quaternion.Euler(0f, randomAngle, 0f);
            }

            Vector3 rotatedForward = randomYRotation * transform.forward;
            Quaternion surfaceRotation = Quaternion.LookRotation(rotatedForward, surfacePoint.normal);

            RotateTo(surfaceRotation);
        }

        public virtual void Dead()
        {
            entityModel.Dead();
            Destroy(gameObject);
        }
        #endregion

        #region View Callback
        private void OnPosition(Vector3 position)
            => entityView.onPosition.Invoke(this, position);
        private void OnRotation(Quaternion rotation)
            => entityView.onRotation.Invoke(this, rotation);
        private void OnDead()
            => entityView.onDead.Invoke(this);
        #endregion
    }
}