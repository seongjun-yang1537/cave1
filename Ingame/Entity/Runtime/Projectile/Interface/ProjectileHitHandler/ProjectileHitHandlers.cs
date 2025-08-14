namespace Ingame
{
    public static class ProjectileHitHandlers
    {
        public static readonly IProjectileHitHandler Damage = new DamageProjectileHitHandler();
        public static readonly IProjectileHitHandler Stun = new StunProjectileHitHandler();
    }
}