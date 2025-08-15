using Core;
using Corelib.Utils;
using UnityEngine;
using VContainer;

namespace Ingame
{
    [RequireComponent(typeof(WorldItemScope))]
    public class WorldItemController : ItemControllerBase
    {
        [Inject] public readonly WorldItemView itemView;
        [Inject] private readonly Rigidbody rigidbody;
        [Inject] private readonly SphereCollider sphereCollider;
        [Inject] public readonly WorldItemType worldItemType;

        public float spawnTime;

        protected override void OnEnable()
        {
            base.OnEnable();

            rigidbody.isKinematic = worldItemType == WorldItemType.HeldItem;
            sphereCollider.enabled = worldItemType != WorldItemType.HeldItem;
            gameObject.SetLayerRecursively(LayerMask.NameToLayer("Entity"));

            spawnTime = Time.time;

            if (worldItemType == WorldItemType.DropItem)
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
