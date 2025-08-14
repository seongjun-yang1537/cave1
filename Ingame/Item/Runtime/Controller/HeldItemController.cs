using Core;
using Corelib.Utils;
using UnityEngine;
using VContainer;

namespace Ingame
{
    [RequireComponent(typeof(HeldItemScope))]
    public class HeldItemController : ControllerBaseBehaviour
    {
        [Inject] public readonly ItemModel itemModel;
        [Inject] public readonly HeldItemView itemView;

        protected override void Awake()
        {
            base.Awake();
            gameObject.SetLayerRecursively(LayerMask.NameToLayer("Entity"));
        }
    }
}
