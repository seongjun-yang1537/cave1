using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace PathX
{
    public class PathXSystem : Singleton<PathXSystem>
    {
        [SerializeField]
        public PathXEngine Engine;
        public MeshFilter meshFilter;

        public static void SetEngine(PathXEngine newEngine)
        {
            Instance.Engine = newEngine;
        }

        public Mesh GetDebugMesh()
        {
            if (meshFilter == null)
            {
                // DEBUG
                GameObject dungeon = GameObject.Find("DungeonModel");
                if (dungeon != null)
                    meshFilter = dungeon.GetComponent<MeshFilter>();
            }
            return meshFilter?.sharedMesh;
        }

        public void ClearEngine()
        {
            Engine = null;
        }

        public static PathXNavMesh GetNavMesh(TriangleDomain navDomain)
        {
            if (Instance.Engine == null || Instance.Engine.Profiles.Count() == 0) return null;
            if (!Instance.Engine.HasProfile(navDomain))
            {
                Debug.LogError($"Not Profile in {navDomain}");
            }
            return Instance.Engine[navDomain];
        }

        public static List<NavSurfacePoint> Pathfind(TriangleDomain navDomain, Vector3 startPoint, Vector3 endPoint)
            => Astar.FindPathCorners(Instance.Engine[navDomain], startPoint, endPoint);

        public static Vector3 PointLocation(TriangleDomain navDomain, Vector3 position)
        {
            PTriangle triangle = TriangleLocation(navDomain, position);
            if (triangle == null) return position;
            return triangle.ClosestPointOnTriangle(position);
        }

        public static PTriangle TriangleLocation(TriangleDomain navDomain, Vector3 position)
            => Instance.Engine?[navDomain]?.TriangleLocation(position);

        public List<PTriangle> BoxLocation(TriangleDomain navDomain, PBox box)
            => Instance.Engine?[navDomain]?.BoxLocation(box);

        public PTriangle PickRandomTriangleInBox(TriangleDomain navDomain, PBox box, MT19937 rng = null)
            => Instance.Engine?[navDomain]?.PickRandomTriangleInBox(box, rng);

        public PTriangle PickRandomTriangleNearby(TriangleDomain navDomain, Vector3 center, float range, MT19937 rng = null)
            => Instance.Engine?[navDomain]?.PickRandomTriangleNearby(center, range, rng);

        public static async UniTask<List<NavSurfacePoint>> PathfindAsync(TriangleDomain navDomain, Vector3 startPoint, Vector3 endPoint)
            => await PathfindAsync(navDomain, startPoint, endPoint, PathfindingSettings.Default);

        public static async UniTask<List<NavSurfacePoint>> PathfindAsync(TriangleDomain navDomain, Vector3 startPoint, Vector3 endPoint, PathfindingSettings settings)
        {
            var navMesh = GetNavMesh(navDomain);
            if (navMesh == null) return null;

            await UniTask.SwitchToThreadPool();
            List<NavSurfacePoint> path = navMesh.Pathfind(startPoint, endPoint, settings);
            await UniTask.SwitchToMainThread();
            return path;
        }
    }
}