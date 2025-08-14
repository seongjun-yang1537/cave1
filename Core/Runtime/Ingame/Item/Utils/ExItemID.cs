namespace Ingame
{
    public static class ExItemID
    {
        public static EquipmentType GetEquipmentType(this ItemID itemID)
        {
            switch (itemID)
            {
                case ItemID.Leather_Helmet:
                    return EquipmentType.Helmet;
                case ItemID.Leather_Chestplate:
                    return EquipmentType.Chestplate;
                case ItemID.Leather_Leggins:
                    return EquipmentType.Leggings;
                case ItemID.Leather_Boots:
                    return EquipmentType.Boots;
            }

            return EquipmentType.None;
        }

        public static ItemType GetItemType(this ItemID itemID)
        {
            switch (itemID)
            {
                case ItemID.Leather_Boots:
                case ItemID.Leather_Chestplate:
                case ItemID.Leather_Ring:
                case ItemID.Leather_Helmet:
                case ItemID.Leather_Leggins:
                case ItemID.Leather_Pandent:
                    return ItemType.Equipment;
                case ItemID.Wood_Sword:
                case ItemID.Diamond_Sword:
                case ItemID.Wand:
                    return ItemType.Weapon;
                case ItemID.Iron_Pickaxe:
                case ItemID.Iron_Axe:
                case ItemID.Iron_Shovel:
                case ItemID.Rope:
                case ItemID.Torch:
                    return ItemType.Tool;
                case ItemID.HPPortion:
                    return ItemType.Consumable;
                case ItemID.Diamond:
                case ItemID.Crystal:
                case ItemID.Gold:
                case ItemID.ScrapMetal:
                    return ItemType.Misc;
            }

            return ItemType.Misc;
        }
    }
}
