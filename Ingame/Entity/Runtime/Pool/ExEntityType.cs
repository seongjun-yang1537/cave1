namespace Ingame
{
    public static class ExEntityType
    {
        public static EntityCategory GetCategory(this EntityType entityType)
            => EntityDB.GetCategory(entityType);
    }
}
