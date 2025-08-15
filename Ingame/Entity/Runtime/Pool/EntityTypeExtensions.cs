namespace Ingame
{
    public static class EntityTypeExtensions
    {
        public static EntityCategory GetCategory(this EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Player:
                    return EntityCategory.Player;
                case EntityType.GoldOre:
                case EntityType.DiamondOre:
                case EntityType.CrystalOre:
                    return EntityCategory.Ore;
                case EntityType.PRJ_RuptureBlast:
                case EntityType.PRJ_ToxicSpit:
                case EntityType.PRJ_Melee:
                case EntityType.PRJ_StunSphere:
                    return EntityCategory.Projectile;
                case EntityType.DropItem:
                    return EntityCategory.DropItem;
                case EntityType.OBJ_Hatch:
                case EntityType.OBJ_Gateway:
                case EntityType.OBJ_FuelTank:
                case EntityType.OBJ_OxygenTank:
                case EntityType.OBJ_ScrapMetal:
                case EntityType.ENV_Plant:
                case EntityType.OBJ_DeliveryBox:
                    return EntityCategory.Environment;
                default:
                    return EntityCategory.Monster;
            }
        }
    }
}
