using Core;
using VContainer;

namespace Ingame
{
    public abstract class ItemControllerBase : ControllerBaseBehaviour
    {
        [Inject] public readonly ItemModel itemModel;
    }
}
