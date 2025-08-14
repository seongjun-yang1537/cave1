using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    // TODO: 사다리꼴로 더 정확하게?
    public class VisibleEnemyAimTargetProvider : IAimTargetProvider
    {
        [Inject] private readonly Transform transform;
        [Inject] private readonly AgentModel agentModel;

        // private float enemyDetectionRange { get => agentModel.enemyDetectionRange; }
        private float enemyDetectionRange = 100f;

        public int GetAimtarget()
        {
            AgentController nowAimtarget = null;

            var enemies = EntitySystem.Entities
                .Where(entity => entity is PlayerController)
                .Where(entity =>
                {
                    float sqrDist = (transform.position - entity.transform.position).sqrMagnitude;
                    return sqrDist <= enemyDetectionRange * enemyDetectionRange;
                });

            int landscapeMask = 1 << LayerMask.NameToLayer("Landscape");
            foreach (var enemy in enemies)
            {
                Vector3 dir = (enemy.transform.position - transform.position).normalized;
                float dist = Vector3.Distance(transform.position, enemy.transform.position);

                if (!Physics.Raycast(transform.position, dir, dist, landscapeMask))
                {
                    nowAimtarget = enemy as AgentController;
                    break;
                }
            }

            if (nowAimtarget == null) return -1;
            return nowAimtarget.entityModel.entityID;
        }
    }
}