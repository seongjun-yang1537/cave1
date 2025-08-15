using Ingame;

namespace UI
{
    public class ShopSellContextUIModel : ContextUIModel
    {
        public InventorySlotModel slotModel;
        public int price;

        public ShopSellContextUIModel(UIMonoBehaviour bindUI) : base(bindUI)
        {
        }
    }
}
