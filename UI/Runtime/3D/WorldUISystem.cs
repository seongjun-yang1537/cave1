using Corelib.Utils;
using Ingame;
using UnityEngine;

namespace UI
{
    public class WorldUISystem : Singleton<WorldUISystem>
    {
        private EntityHUDPool entityHUDPool;
        private DamageIndicatorPool damageIndicatorPool;

        private void Awake()
        {
            entityHUDPool = GetComponentInChildren<EntityHUDPool>();
            damageIndicatorPool = GetComponentInChildren<DamageIndicatorPool>();
        }

        protected void OnEnable()
        {
            EntitySystem.Instance.onSpawn.AddListener(OnSpawnEntity);
        }

        protected void OnDisable()
        {
            EntitySystem.Instance.onSpawn.RemoveListener(OnSpawnEntity);
        }

        private void OnSpawnEntity(EntityController entityController)
        {
            if (entityController is not AgentController) return;
            if (entityController is PlayerController) return;

            AgentController agentController = entityController as AgentController;
            entityHUDPool?.Spawn(agentController);
        }

        public void SpawnDamageIndicator(Vector3 position, float damage)
        {
            damageIndicatorPool?.Spawn(position, damage);
        }
    }
}