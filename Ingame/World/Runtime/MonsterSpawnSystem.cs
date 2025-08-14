using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Corelib.Utils;
using World;
using Ingame;
using PathX;
using Core;

namespace World
{
    public class MonsterSpawnSystem : Singleton<MonsterSpawnSystem>
    {
        private List<MonsterSpawnTrigger> spawnTriggers = new();
        private HashSet<int> triggered = new();

        private static List<PTriangleGraphNode> GetFarthestTriangles(PathXNavMesh navMesh, Vector3 startPoint, float maxDistance, int count)
        {
            if (navMesh == null || count <= 0)
            {
                return new List<PTriangleGraphNode>();
            }

            var triangles = navMesh.TrianglesWithinDistance(startPoint, maxDistance);

            return triangles.OrderByDescending(t => Vector3.Distance(t.center, startPoint))
                            .Take(count)
                            .ToList();
        }

        protected void OnEnable()
        {
            if (GameWorld.Instance != null)
            {
                GameWorld.Instance.onWorldData.AddListener(OnWorldData);
                if (GameWorld.Instance.worldData != null)
                    OnWorldData(GameWorld.Instance.worldData);
            }
        }

        protected void OnDisable()
        {
            if (GameWorld.Instance != null)
                GameWorld.Instance.onWorldData.RemoveListener(OnWorldData);
        }

        private void OnWorldData(WorldData worldData)
        {
            spawnTriggers = worldData?.monsterSpawnTriggers ?? new();
            triggered.Clear();
        }

        private void Update()
        {
            if (spawnTriggers == null || spawnTriggers.Count == 0)
                return;

            foreach (var player in PlayerSystem.Players)
            {
                Vector3 pos = player.transform.position;
                for (int i = 0; i < spawnTriggers.Count; i++)
                {
                    if (triggered.Contains(i))
                        continue;

                    var trigger = spawnTriggers[i];
                    float rSq = trigger.radius * trigger.radius;
                    if ((pos - trigger.center).sqrMagnitude <= rSq)
                    {
                        triggered.Add(i);

                        var navMesh = PathXSystem.GetNavMesh(TriangleDomain.All);
                        // TODO
                        float maxDist = 12f;
                        // float minDist = trigger.radius * 0.7f;
                        var triangles = GetFarthestTriangles(navMesh, pos, maxDist, 20);

                        if (triangles.Count == 0)
                            triangles = GetFarthestTriangles(navMesh, pos, maxDist, 20);

                        foreach (var monster in trigger.monsters)
                        {
                            if (triangles.Count == 0)
                                break;

                            var tri = triangles[GameRng.Game.NextInt(0, triangles.Count - 1)];

                            var controller = EntitySystem.SpawnEntity(monster) as EntityController;
                            if (controller == null) continue;

                            controller.transform.position = tri.center;
                            controller.Spawn();
                            controller.SnapToNavMesh();
                        }
                    }
                }
            }
        }
    }
}
