using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class StunProjectileHitHandler : IProjectileHitHandler
    {
        public void Hit(ProjectileController projectile, EntityController target)
        {
            if (target is AgentController)
                ApplyStun(projectile, target as AgentController);
            ProjectileHitHandlers.Damage.Hit(projectile, target);
        }

        private void ApplyStun(ProjectileController projectile, AgentController target)
        {
            target.AddStatusEffect(new StunStatusEffect());
        }
    }
}