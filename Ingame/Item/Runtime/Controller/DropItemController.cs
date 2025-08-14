using Core;
using Corelib.Utils;
using UnityEngine;
using VContainer;

namespace Ingame
{
    [RequireComponent(typeof(DropItemScope))]
    public class DropItemController : ItemControllerBase
    {
        [Inject] public readonly DropItemView itemView;
        [Inject] private readonly Rigidbody rigidbody;
        [Inject] private readonly SphereCollider sphereCollider;
        public float spawnTime;

        protected override void Awake()
        {
            base.Awake();

            rigidbody.isKinematic = false;
            sphereCollider.enabled = true;
            gameObject.SetLayerRecursively(LayerMask.NameToLayer("Entity"));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            spawnTime = Time.time;
            Leap();
        }

        public void Leap()
        {
            float force = 3f;
            Vector3 upVector = (Random.insideUnitSphere + Vector3.up * 5).normalized;
            float randomForce = GameRng.Game.NextFloat(force, force * 1.5f);
            rigidbody.AddForce(upVector * randomForce, ForceMode.Impulse);
        }
    }
}

