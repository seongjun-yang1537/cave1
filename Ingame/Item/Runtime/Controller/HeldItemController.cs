using Core;
using Corelib.Utils;
using UnityEngine;
using VContainer;

namespace Ingame
{
    [RequireComponent(typeof(HeldItemScope))]
    public class HeldItemController : ItemControllerBase
    {
        [Inject] public readonly HeldItemView itemView;
        [Inject] private readonly Rigidbody rigidbody;
        [Inject] private readonly SphereCollider sphereCollider;

        protected override void Awake()
        {
            base.Awake();
            rigidbody.isKinematic = true;
            sphereCollider.enabled = false;
            gameObject.SetLayerRecursively(LayerMask.NameToLayer("Entity"));
        }
    }
}
