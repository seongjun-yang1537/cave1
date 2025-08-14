namespace Ingame
{
    public static class ExEquipmentSlotID
    {
        public static EquipmentType ToType(this EquipmentSlotID id)
        {
            return id switch
            {
                EquipmentSlotID.Boots => EquipmentType.Boots,
                EquipmentSlotID.Chestplate => EquipmentType.Chestplate,
                EquipmentSlotID.Helmet => EquipmentType.Helmet,
                EquipmentSlotID.Leggings => EquipmentType.Leggings,
                EquipmentSlotID.OxygenTank => EquipmentType.OxygenTank,
                EquipmentSlotID.Jetpack => EquipmentType.Jetpack,
                EquipmentSlotID.Bag => EquipmentType.Bag,
                _ => (EquipmentType)(-1),
            };
        }
    }
}