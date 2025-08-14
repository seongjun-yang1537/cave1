using Corelib.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using VContainer;
using System.Collections;

namespace Ingame
{
    [RequireComponent(typeof(PawnScope))]
    public class PawnController : AgentController
    {
        [ModelSourceBase]
        public PawnModel pawnModel;
        public PawnView pawnView;

        [Inject] protected readonly CharacterController characterController;
        [Inject] public readonly HandController handController;

        [Inject] protected readonly IMoveable moveable;

        private Coroutine knockbackCoroutine;

        #region LifeCycle
        protected override void Awake()
        {
            base.Awake();

            pawnModel = (PawnModel)agentModel;
            pawnView = (PawnView)agentView;
        }

        protected override void Update()
        {
            base.Update();
            moveable?.Update();
            UpdateMovement();
            UpdateRotation();
        }

        protected void FixedUpdate()
        {
            moveable?.FixedUpdate();
        }
        #endregion

        #region Action
        public virtual void AcquireItem(ItemModel itemModel)
        {
            pawnModel.inventory.AddItem(itemModel);
        }

        public virtual void DiscardItem(ItemModel itemModel)
        {
            pawnModel.inventory.RemoveItem(itemModel);
        }

        public virtual void UpdateMovement()
        {
            if (moveable == null) return;
            Vector3? nextPosition = moveable.GetNextPosition();
            if (nextPosition == null) return;
            MoveTo(nextPosition.Value);
        }

        public virtual void UpdateRotation()
        {
            if (moveable == null) return;
            Quaternion? nextRotation = moveable.GetNextRotation();
            if (nextRotation == null) return;
            RotateTo(nextRotation.Value);
        }

        public virtual void Jump()
        {
            moveable?.Jump();
        }

        public virtual bool IsGrounded()
        {
            return moveable?.IsGrounded() ?? false;
        }

        public virtual void ChangeHeldItem(InventorySlotModel itemSlot)
        {
            pawnModel.SetHeldItem(itemSlot);
        }

        public virtual void DropItem(InventorySlotModel itemSlot)
        {
            pawnModel.DropItem(itemSlot);
        }

        public virtual void ChangePose(PawnPoseState poseState)
        {
            pawnModel.SetPoseState(poseState);
        }

        public void ApplyKnockback(Vector3 direction, float force)
        {
            if (knockbackCoroutine != null)
                StopCoroutine(knockbackCoroutine);

            knockbackCoroutine = StartCoroutine(KnockbackRoutine(direction, force));
        }

        private IEnumerator KnockbackRoutine(Vector3 direction, float force)
        {
            Vector3 velocity = direction * force;
            const float deceleration = 20f;

            while (velocity.sqrMagnitude > 0.01f)
            {
                characterController.Move(velocity * Time.deltaTime);
                velocity = Vector3.MoveTowards(velocity, Vector3.zero, deceleration * Time.deltaTime);
                yield return null;
            }
        }
        #endregion

        #region Event Callback
        [AutoModelSubscribe(nameof(PawnModel.onPoseState))]
        protected virtual void OnPoseState(PawnPoseState poseState)
            => pawnView.onPoseState.Invoke(this, poseState);

        [AutoModelSubscribe(nameof(PawnModel.onHeldItem))]
        protected virtual void OnHeldItem(InventorySlotModel itemSlot)
            => pawnView.onHeldItem.Invoke(this, itemSlot.itemModel);

        [AutoModelSubscribe(nameof(PawnModel.onDropItem))]
        protected virtual void OnDropItem(ItemModel itemModel)
            => pawnView.onDropItem.Invoke(this, itemModel);
        #endregion
    }
}