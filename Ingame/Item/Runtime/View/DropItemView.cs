using Corelib.Utils;
using UnityEngine;
using VContainer;

namespace Ingame
{
    [RequireComponent(typeof(DropItemScope))]
    public class DropItemView : MonoBehaviour
    {
        [Inject] private ItemModel itemModel;
        [Inject] private ItemDropAnimation dropAnimation;
    }
}

