using Core;
using Corelib.Utils;
using UnityEngine;
using VContainer;

namespace Ingame
{
    public abstract class ItemControllerBase : ControllerBaseBehaviour
    {
        [Inject] public readonly ItemModel itemModel;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (ItemSystem.Instance != null)
                ItemSystem.Instance.Remove(this);
        }
    }
}
