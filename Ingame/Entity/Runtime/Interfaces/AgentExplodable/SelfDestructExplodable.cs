using UnityEngine;

namespace Ingame
{
    public class SelfDestructExplodable : IAgentExplodable
    {
        public void Explode(AgentController agentController)
        {
            AgentModel agentModel = agentController.agentModel;
            switch (agentModel.entityType)
            {
                case EntityType.Rupture:
                    SpawnRuptureProjectile(agentController);
                    break;
            }
            Object.Destroy(agentController.gameObject);
        }

        private void SpawnRuptureProjectile(AgentController agentController)
        {
            AgentModel agentModel = agentController.agentModel;

            ProjectileContext context = new ProjectileContext
                .Builder(EntityType.PRJ_RuptureBlast)
                .SetOwner(agentModel)
                .SetLifeTime(3f)
                .SetSpeed(0f)
                .SetDamage(agentModel.totalStat.attack)
                .Build();

            ProjectileController controller = ProjectileSystem.SpawnProjectile(context);
            controller.transform.position = agentController.transform.position;
        }
    }
}