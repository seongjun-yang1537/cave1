using GoogleSheet.Core.Type;

namespace Ingame
{
    [UGS(typeof(EquipmentType))]
    public enum EquipmentType
    {
        None,
        Helmet,
        Chestplate,
        Leggings,
        Boots,
        Jetpack,
        OxygenTank,
        Bag,
        COUNT,
    }
}