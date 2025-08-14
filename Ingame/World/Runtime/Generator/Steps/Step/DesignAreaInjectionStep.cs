using Cysharp.Threading.Tasks;
using Corelib.Utils;
using UnityEngine;
using VoxelEngine;

namespace World
{
    public class DesignAreaInjectionStep : IWorldgenDesignStep
    {
        private readonly DesignArea area;
        private readonly Vector3 offset;

        public DesignAreaInjectionStep(DesignArea area, Vector3 offset)
        {
            this.area = area;
            this.offset = offset;
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            var field = worldData?.chunkedScalarField;
            if (field == null || area == null) return UniTask.CompletedTask;

            foreach (var box in area.Boxes)
            {
                var b = new PBox(box.center + offset, box.size);
                Vector3Int min = Vector3Int.FloorToInt(b.Min);
                Vector3Int max = Vector3Int.CeilToInt(b.Max) - Vector3Int.one;
                min = Vector3Int.Max(min, Vector3Int.zero);
                max = Vector3Int.Min(max, field.Size - Vector3Int.one);
                for (int x = min.x; x <= max.x; x++)
                    for (int y = min.y; y <= max.y; y++)
                        for (int z = min.z; z <= max.z; z++)
                            field[x, y, z] = 0;
            }

            foreach (var sphere in area.Spheres)
            {
                var s = new PSphere(sphere.center + offset, sphere.radius);
                Vector3Int min = Vector3Int.FloorToInt(s.Min);
                Vector3Int max = Vector3Int.CeilToInt(s.Max) - Vector3Int.one;
                min = Vector3Int.Max(min, Vector3Int.zero);
                max = Vector3Int.Min(max, field.Size - Vector3Int.one);
                float r2 = s.radius * s.radius;
                for (int x = min.x; x <= max.x; x++)
                    for (int y = min.y; y <= max.y; y++)
                        for (int z = min.z; z <= max.z; z++)
                        {
                            Vector3 diff = new Vector3(x, y, z) - s.center;
                            if (diff.sqrMagnitude <= r2)
                                field[x, y, z] = 0;
                        }
            }

            return UniTask.CompletedTask;
        }
    }
}
