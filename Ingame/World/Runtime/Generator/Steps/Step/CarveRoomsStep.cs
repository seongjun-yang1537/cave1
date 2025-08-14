using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Corelib.Utils;
using Unity.Mathematics;
using UnityEngine;
using VoxelEngine;
using System;

namespace World
{
    /*
    * 절차적 동굴 생성 단계: 구(Sphere) 형태로 공간을 깎아 방(Room)을 생성합니다.
    * 이 작업은 아래와 같은 절차로 Job System을 통해 병렬 처리됩니다.
    *
    * 1. 데이터 수집
    * - 월드에 존재하는 모든 Room 정보를 기반으로, 깎아낼 구(Sphere)들의 파라미터(중심점, 반지름, 노이즈 시드 등)를
    * 하나의 리스트 (`NativeArray<SphereParams>`)에 모두 수집합니다.
    *
    * 2. Job 스케줄링
    * - 로드된 모든 청크(Chunk)를 순회하며, 각 청크마다 `CarveChunkJob`을 하나씩 생성하고 실행합니다.
    * - 각 Job은 (1)에서 만든 전체 구 목록과 (2)자신이 담당할 청크의 데이터 복사본을 전달받습니다.
    * 청크 데이터는 복사본이므로 Job 간 데이터 경합(Race Condition)이 발생하지 않습니다.
    *
    * 3. Job 내부 실행 로직 (핵심 최적화)
    * - Job은 전달받은 **전체 구 목록을 먼저 순회**합니다.
    * - 각 구가 현재 Job의 청크 경계와 겹치는지 빠르게 확인하여, **관련 없는 구는 즉시 계산에서 제외**합니다.
    * - 경계가 겹치는 구에 한해서만, 해당 영역의 복셀들을 대상으로 실제 Carve 연산(거리, 노이즈 계산)을 수행합니다.
    * 이 '구체 중심(Sphere-Centric)' 방식은 연산량을 크게 줄여줍니다.
    *
    * 4. 결과 적용
    * - 메인 스레드는 모든 Job이 완료될 때까지 대기합니다 (`JobHandle.CompleteAll`).
    * - 작업이 완료된 후, 수정된 모든 청크 데이터들을 실제 월드 데이터에 적용하고 할당된 메모리를 해제합니다.
    */
    [Serializable]
    public class CarveRoomsStep : IWorldgenRasterizeStep
    {
        private readonly float noiseScale;
        private readonly float radiusVariance;
        private readonly int minSpheres;
        private readonly int maxSpheres;

        public CarveRoomsStep(float noiseScale = 0.1f,
                              float radiusVariance = 10f,
                              int minSpheres = 2,
                              int maxSpheres = 4)
        {
            this.noiseScale = noiseScale;
            this.radiusVariance = math.abs(radiusVariance);
            this.minSpheres = math.max(1, minSpheres);
            this.maxSpheres = math.max(this.minSpheres, maxSpheres);
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            var field = worldData?.chunkedScalarField;
            var graph = worldData?.graph;
            if (field == null || graph?.nodes == null) return UniTask.CompletedTask;

            foreach (var node in graph.nodes)
            {
                if (node.room == null) continue;
                ClearRoom(field, rng, node.room);
            }

            return UniTask.CompletedTask;
        }

        private void ClearRoom(ChunkedScalarField field, MT19937 rng, PBoxInt room)
        {
            int count = 10 * rng.NextInt(minSpheres, maxSpheres + 1);
            for (int i = 0; i < count; i++)
            {
                float3 center = new float3(
                    rng.NextFloat(room.topLeft.x, room.bottomRight.x),
                    rng.NextFloat(room.topLeft.y, room.bottomRight.y),
                    rng.NextFloat(room.topLeft.z, room.bottomRight.z));
                float rad = math.min(room.lenX, room.lenZ) * 0.5f;
                rad += rng.NextFloat(-radiusVariance, radiusVariance);

                ClearSphere(field, rng, center, rad, room.topLeft.y);
            }
        }

        private void ClearSphere(ChunkedScalarField field, MT19937 rng, float3 center, float r, int floorY)
        {
            Vector3Int min = new Vector3Int(
                Mathf.FloorToInt(center.x - r),
                Mathf.FloorToInt(center.y - r),
                Mathf.FloorToInt(center.z - r));
            Vector3Int max = new Vector3Int(
                Mathf.CeilToInt(center.x + r),
                Mathf.CeilToInt(center.y + r),
                Mathf.CeilToInt(center.z + r));

            min = Vector3Int.Max(min, Vector3Int.zero);
            max = Vector3Int.Min(max, field.Size - Vector3Int.one);

            float r2 = r * r;
            float3 seed = rng.NextVector3();

            for (int x = min.x; x <= max.x; x++)
                for (int y = min.y; y <= max.y; y++)
                    for (int z = min.z; z <= max.z; z++)
                    {
                        if (y <= floorY)
                        {
                            field[x, y, z] = 0;
                            continue;
                        }

                        float3 wp = new float3(x, y, z);
                        float dist2 = math.lengthsq(wp - center);
                        if (dist2 > r2) continue;
                        float dist = math.sqrt(dist2);
                        float n = noise.cnoise(wp * noiseScale + seed) * 0.5f + 0.5f;
                        if (n > 1f - dist / r) continue;
                        field[x, y, z] = 0;
                    }
        }
    }
}

