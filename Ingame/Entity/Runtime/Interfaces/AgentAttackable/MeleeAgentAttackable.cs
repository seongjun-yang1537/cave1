using UnityEngine;

namespace Ingame
{
    public class MeleeAgentAttackable : IAgentAttackable
    {
        public float Attack(AgentController attacker, AgentController target, AttackContext info)
        {
            AgentModel agentModel = attacker.agentModel;
            switch (agentModel.entityType)
            {
                case EntityType.Knocker:
                    SpawnKnockerProjectile(attacker, target, info);
                    break;
                case EntityType.Ant:
                    SpawnAntProjectile(attacker, target, info);
                    break;
                case EntityType.DragonBoar:
                    SpawnDragonBoarProjectile(attacker, target, info);
                    break;
            }
            return info.damage;
        }

        private void SpawnKnockerProjectile(AgentController attacker,
                                            AgentController target,
                                            AttackContext info)
        {
            Vector3 attackPosition = attacker.transform.position;

            var context = new ProjectileContext
                .Builder(EntityType.PRJ_Melee)
                .SetOwner(attacker.agentModel)
                .SetFollowTarget(target.transform)
                .SetSpeed(100f)
                .SetDamage(info.damage)
                .OnHitTarget(() =>
                {
                    var pawnController = target as PawnController;
                    if (!pawnController) return;

                    Vector3 dir = (target.transform.position - attackPosition).normalized;
                    Vector3 knockDir = (dir + Vector3.up * 0.5f).normalized;

                    float baseForce = 20f;
                    float dmgFactor = info.damage * 0.1f;
                    float forceMag = baseForce + dmgFactor;

                    pawnController.ApplyKnockback(knockDir, forceMag);
                })
                .Build();

            var prj = ProjectileSystem.SpawnProjectile(context);
            prj.transform.position = attacker.transform.position;
        }

        private void SpawnAntProjectile(AgentController attacker,
                                    AgentController target,
                                    AttackContext info)
        {
            var context = new ProjectileContext
                .Builder(EntityType.PRJ_Melee)
                .SetOwner(attacker.agentModel)
                .SetFollowTarget(target.transform)
                .SetSpeed(100f)
                .SetDamage(info.damage)
                .Build();

            var prj = ProjectileSystem.SpawnProjectile(context);
            prj.transform.position = attacker.transform.position;
        }

        private void SpawnDragonBoarProjectile(AgentController attacker,
                            AgentController target,
                            AttackContext info)
        {
            var context = new ProjectileContext
                .Builder(EntityType.PRJ_Melee)
                .SetOwner(attacker.agentModel)
                .SetFollowTarget(target.transform)
                .SetSpeed(100f)
                .SetDamage(info.damage)
                .Build();

            var prj = ProjectileSystem.SpawnProjectile(context);
            prj.transform.position = attacker.transform.position;
        }
    }
}