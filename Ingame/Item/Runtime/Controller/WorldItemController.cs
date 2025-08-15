using Core;
using Corelib.Utils;
using UnityEngine;
using VContainer;

namespace Ingame
{
    public enum WorldItemMode
    {
        Drop,
        Held,
    }

    [RequireComponent(typeof(WorldItemScope))]
    public class WorldItemController : ItemControllerBase
    {
        [Inject] public readonly WorldItemView itemView;
        [Inject] private readonly Rigidbody rigidbody;
        [Inject] private readonly SphereCollider sphereCollider;

        public float spawnTime;
        public WorldItemMode Mode { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            gameObject.SetLayerRecursively(LayerMask.NameToLayer("Entity"));
        }

        public void SetMode(WorldItemMode newMode)
        {
            Mode = newMode;
            switch (newMode)
            {
                case WorldItemMode.Drop:
                    rigidbody.isKinematic = false;
                    sphereCollider.enabled = true;
                    spawnTime = Time.time;
                    Leap();
                    break;
                case WorldItemMode.Held:
                    rigidbody.isKinematic = true;
                    sphereCollider.enabled = false;
                    break;
            }
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
