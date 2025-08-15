using Corelib.Utils;
using UnityEngine;
using VContainer;

namespace Ingame
{
    [RequireComponent(typeof(WorldItemScope))]
    public class WorldItemView : MonoBehaviour
    {
        [Inject] private ItemModel itemModel;
        [Inject] private ItemDropAnimation dropAnimation;
    }
}
