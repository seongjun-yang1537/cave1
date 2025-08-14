namespace Ingame
{
    public interface IProjectileHitHandler
    {
        public void Hit(ProjectileController projectile, EntityController target);
    }
}