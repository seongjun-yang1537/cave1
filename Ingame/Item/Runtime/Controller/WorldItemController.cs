using Core;
using Corelib.Utils;
using UnityEngine;
using VContainer;

namespace Ingame
{
    [RequireComponent(typeof(WorldItemScope))]
    public class WorldItemController : ItemControllerBase
    {
        public enum Mode
        {
            Drop,
            Held,
        }

        [Inject] public readonly WorldItemView itemView;
        [Inject] private readonly Rigidbody rigidbody;
        [Inject] private readonly SphereCollider sphereCollider;

        public float spawnTime;
        private Mode _currentMode;

        protected override void Awake()
        {
            base.Awake();
            gameObject.SetLayerRecursively(LayerMask.NameToLayer("Entity"));
        }

        public void SetMode(Mode newMode)
        {
            _currentMode = newMode;
            switch (newMode)
            {
                case Mode.Drop:
                    rigidbody.isKinematic = false;
                    sphereCollider.enabled = true;
                    spawnTime = Time.time;
                    Leap();
                    break;
                case Mode.Held:
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
