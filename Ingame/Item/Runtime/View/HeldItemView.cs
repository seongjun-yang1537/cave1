using Corelib.Utils;
using UnityEngine;
using VContainer;

namespace Ingame
{
    [RequireComponent(typeof(HeldItemScope))]
    public class HeldItemView : MonoBehaviour
    {
        [Inject] private ItemModel itemModel;
    }
}
