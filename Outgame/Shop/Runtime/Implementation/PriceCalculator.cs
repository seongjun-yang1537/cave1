using System.Collections.Generic;
using Ingame;

namespace Outgame
{
    public class PriceCalculator : IPriceCalculator
    {
        private static readonly Dictionary<ItemID, int> PriceTable = new()
        {
            { ItemID.Leather_Boots, 150 },
            { ItemID.Leather_Chestplate, 300 },
            { ItemID.Leather_Ring, 50 },
            { ItemID.Leather_Helmet, 100 },
            { ItemID.Leather_Leggins, 200 },
            { ItemID.Leather_Pandent, 75 },
            { ItemID.Diamond, 1000 },
            { ItemID.Wood_Sword, 50 },
            { ItemID.Crystal, 500 },
            { ItemID.Iron_Pickaxe, 150 },
            { ItemID.Iron_Axe, 120 },
            { ItemID.Iron_Shovel, 100 },
            { ItemID.Rope, 20 },
            { ItemID.Torch, 10 },
            { ItemID.Gold, 0 }, // Gold is currency, so it shouldn't have a price
            { ItemID.ScrapMetal, 5 },
            { ItemID.Wand, 250 },
            { ItemID.Diamond_Sword, 2000 },
            { ItemID.HPPortion, 50 },
        };

        public int GetPrice(ShopItemModel shopItemModel)
        {
            if (PriceTable.TryGetValue(shopItemModel.itemModel.Data.itemID, out var price))
            {
                return price;
            }

            // Return a default price for items not in the table
            return 100;
        }
    }
}
