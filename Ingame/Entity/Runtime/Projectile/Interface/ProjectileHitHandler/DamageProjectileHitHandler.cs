using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class DamageProjectileHitHandler : IProjectileHitHandler
    {
        public void Hit(ProjectileController projectile, EntityController target)
        {
            ProjectileModel projectileModel = projectile.projectileModel;

            EntityController attacker = EntitySystem.FindController(projectileModel.owner);
            AgentModel attackerModel = attacker?.entityModel as AgentModel;

            AgentController agentController = target as AgentController;
            if (agentController != null)
            {
                agentController.TakeDamage(attackerModel, projectileModel.damage);
            }
        }
    }
}