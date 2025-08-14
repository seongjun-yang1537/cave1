namespace Ingame
{
    public static class EntityItemActions
    {
        public static readonly IEntityItemAction Default = new DefaultItemAction();
        public static readonly IEntityItemAction Pickaxe = new PickaxeItemAction();
        public static readonly IEntityItemAction Shovel = new ShovelItemAction();
        public static readonly IEntityItemAction Rope = new RopeItemAction();
        public static readonly IEntityItemAction Wand = new WandItemAction();
        public static readonly IEntityItemAction HPPortion = new HPPortionItemAction();
    }
}