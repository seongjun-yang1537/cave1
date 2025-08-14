using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class PawnMoveable : IMoveable, IInitializable
    {
        [Inject] protected readonly PawnModel pawnModel;
        [Inject] protected readonly Transform transform;
        [Inject] protected readonly IDirectionProvider directionProvider;
        protected PawnPhysicsSetting physicsSetting { get => pawnModel.physicsSetting; }

        protected Rigidbody rigidbody;
        protected CapsuleCollider capsuleCollider;

        public virtual void Initialize()
        {
            rigidbody = transform.GetComponent<Rigidbody>();
            capsuleCollider = transform.GetComponent<CapsuleCollider>();
        }

        public virtual void Update()
        {

        }

        public virtual void FixedUpdate()
        {

        }

        public virtual void ClearVelocity() { }

        public virtual Vector3? GetNextPosition() => null;

        public virtual Quaternion? GetNextRotation()
        {
            Vector3 forward = directionProvider.Forward;
            forward.y = 0f;
            forward.Normalize();

            if (forward.sqrMagnitude < 0.01f) return null;

            return Quaternion.LookRotation(forward);
        }

        public virtual void Jump()
        {
            if (!IsGrounded()) return;

            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
            rigidbody.AddForce(
                Vector3.up * pawnModel.pawnTotalStat.jumpForce * physicsSetting.JUMP_FORCE_CONSTANT,
                ForceMode.Impulse
            );
        }

        public virtual bool IsGrounded()
        {
            return Physics.CheckSphere(
                transform.position,
                capsuleCollider.radius,
                LayerMask.GetMask("Landscape")
            );
        }
    }
}