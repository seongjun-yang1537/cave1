namespace Ingame
{
    public static class ExEquipmentType
    {
        public static EquipmentSlotID ToSlotID(this EquipmentType type)
        {
            return type switch
            {
                EquipmentType.Boots => EquipmentSlotID.Boots,
                EquipmentType.Chestplate => EquipmentSlotID.Chestplate,
                EquipmentType.Helmet => EquipmentSlotID.Helmet,
                EquipmentType.Leggings => EquipmentSlotID.Leggings,
                EquipmentType.OxygenTank => EquipmentSlotID.OxygenTank,
                EquipmentType.Jetpack => EquipmentSlotID.Jetpack,
                EquipmentType.Bag => EquipmentSlotID.Bag,
                _ => (EquipmentSlotID)(-1),
            };
        }
    }
}