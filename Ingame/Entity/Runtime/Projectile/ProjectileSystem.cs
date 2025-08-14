using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    public class ProjectileSystem : Singleton<ProjectileSystem>
    {
        public static IEnumerable<ProjectileController> Projectiles
        {
            get
            {
                foreach (Transform child in Instance.transform)
                {
                    if (child == null) continue;

                    var controller = child.GetComponent<ProjectileController>();
                    if (controller != null)
                        yield return controller;
                }
            }
        }

        public UnityEvent<ProjectileController> onSpawn = new();

        public static ProjectileController SpawnProjectile(ProjectileContext context)
        {
            GameObject prefab = EntityDB.LoadPrefab(context.Type);
            var data = EntityDB.LoadModelData(context.Type) as ProjectileModelData;
            var templateData = EntityModelUtils.CreateState(data) as ProjectileModelState;

            ProjectileModelState state = new ProjectileModelState();
            state.damage = context.Damage;
            state.speed = context.Speed;
            state.lifeTime = context.LifeTime;
            state.range = templateData.range;
            state.targetLayer = context.TargetLayer;
            state.owner = context.Owner;
            state.followTarget = context.FollowTarget;
            state.onHitTarget = context.OnHitTarget;
            state.isSpanwed = true;

            GameObject go = Instantiate(prefab);

            ProjectileScope scope = go.GetComponent<ProjectileScope>();
            scope.entityModelData = data;
            scope.entityModelState = state;
            scope.hitHandler = context.HitHandler;

            Transform tr = go.transform;
            tr.SetParent(Instance.transform);
            tr.Reset();

            ProjectileController controller = go.GetComponent<ProjectileController>();

            Instance.onSpawn.Invoke(controller);

            return controller;
        }
    }
}