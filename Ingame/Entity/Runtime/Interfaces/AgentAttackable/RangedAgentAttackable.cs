using UnityEngine;

namespace Ingame
{
    public class RangedAgentAttackable : IAgentAttackable
    {
        public float Attack(AgentController attacker, AgentController target, AttackContext info)
        {
            AgentModel agentModel = attacker.agentModel;
            switch (agentModel.entityType)
            {
                case EntityType.Creeplinger:
                    SpawnCreeplingerProjectile(attacker, target, info);
                    break;
            }
            return info.damage;
        }

        private void SpawnCreeplingerProjectile(AgentController attacker, AgentController target, AttackContext info)
        {
            ProjectileContext context = new ProjectileContext
                .Builder(EntityType.PRJ_ToxicSpit)
                .SetOwner(attacker.agentModel)
                .SetFollowTarget(target.transform)
                .SetSpeed(3f)
                .SetLifeTime(3f)
                .SetDamage(info.damage)
                .Build();

            ProjectileController controller = ProjectileSystem.SpawnProjectile(context);
            controller.transform.position = attacker.transform.position;
        }
    }
}