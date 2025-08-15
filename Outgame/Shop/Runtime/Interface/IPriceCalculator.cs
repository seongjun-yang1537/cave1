using Ingame;

namespace Outgame
{
    public interface IPriceCalculator
    {
        int GetPrice(ItemModel shopItemModel);
    }
}
