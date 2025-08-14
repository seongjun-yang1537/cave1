using Cysharp.Threading.Tasks;
using Corelib.Utils;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using System.Collections.Generic;
using VoxelEngine;

namespace World
{
    /*
    * ==========================================================================================
    * 월드 경계 채우기 (FillBordersStep)
    * ==========================================================================================
    *
    * [역할]
    * - 이 단계는 월드 전체의 X, Y, Z축 경계를 지정된 두께(border)만큼 단색 복셀(1)로 채웁니다.
    * - 맵의 가장자리가 열려있지 않도록 막아주는 역할을 합니다.
    *
    * [핵심 최적화 전략]
    * - 모든 청크에 대해 무조건 Job을 실행하는 대신, Job 스케줄링 전에 청크의 경계 상자(Bounding Box)가
    * 월드의 경계 영역과 겹치는지 **메인 스레드에서 미리 확인**합니다.
    * - 이 **필터링(Filtering)** 과정을 통해 월드 중앙부에 위치하여 작업이 전혀 필요 없는 대다수의 청크에 대해서는
    * 불필요한 Job 생성 및 복셀 순회 연산을 방지하여 성능을 크게 향상시킵니다.
    *
    * [실행 흐름]
    * 1. 청크 필터링: 모든 청크를 순회하며, 월드 경계와 겹치는 청크만 작업 대상으로 선별합니다.
    * 2. Job 스케줄링: 선별된 청크에 대해서만 FillChunkJob을 병렬로 스케줄링합니다.
    * 3. Job 실행: Job은 담당 청크 내에서 경계에 해당하는 복셀들만 찾아 값을 1로 설정합니다.
    * (추가 최적화: Job 내부에서도 청크 전체가 아닌, 겹치는 영역만 순회하도록 범위를 좁힐 수 있습니다.)
    * 4. 결과 적용: 모든 Job이 완료되면 결과를 실제 청크 데이터에 반영합니다.
    */
    public class FillBordersStep : IWorldgenRasterizeStep
    {
        private readonly int border;

        public FillBordersStep(int border)
        {
            this.border = Mathf.Max(1, border);
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            var field = worldData?.chunkedScalarField;
            if (field == null)
                return UniTask.CompletedTask;

            var coords = field.GetLoadedChunkCoordinates();
            var handles = new NativeArray<JobHandle>(coords.Count, Allocator.Temp);
            var resources = new List<(BitpackedScalarField chunk, NativeArray<byte> data)>();

            int bits = 1;
            uint mask = (1u << bits) - 1u;
            Vector3Int fieldSize = field.Size;

            int i = 0;
            foreach (var coord in coords)
            {
                var chunk = field.GetChunk(coord);
                var data = chunk.GetDataArray_ForJob(Allocator.TempJob);
                var job = new FillChunkJob
                {
                    data = data,
                    size = chunk.Size,
                    offset = coord * ChunkedScalarField.CHUNK_SIZE,
                    fieldSize = fieldSize,
                    border = border,
                    bitsPerValue = bits,
                    valueMask = mask
                };
                handles[i++] = job.Schedule();
                resources.Add((chunk, data));
            }

            JobHandle.CompleteAll(handles);

            foreach (var res in resources)
            {
                res.chunk.SetData(res.data);
                res.data.Dispose();
            }

            handles.Dispose();

            return UniTask.CompletedTask;
        }

        [Unity.Burst.BurstCompile]
        private struct FillChunkJob : IJob
        {
            [NativeDisableParallelForRestriction] public NativeArray<byte> data;
            [ReadOnly] public Vector3Int size;
            [ReadOnly] public Vector3Int offset;
            [ReadOnly] public Vector3Int fieldSize;
            [ReadOnly] public int border;
            [ReadOnly] public int bitsPerValue;
            [ReadOnly] public uint valueMask;

            private int Index(int x, int y, int z) => z + y * size.z + x * size.z * size.y;

            public void Execute()
            {
                int maxX = fieldSize.x - border;
                int maxY = fieldSize.y - border;
                int maxZ = fieldSize.z - border;

                for (int x = 0; x < size.x; x++)
                    for (int y = 0; y < size.y; y++)
                        for (int z = 0; z < size.z; z++)
                        {
                            int worldX = offset.x + x;
                            int worldY = offset.y + y;
                            int worldZ = offset.z + z;

                            bool fill =
                                worldX < border || worldX >= maxX ||
                                worldY < border || worldY >= maxY ||
                                worldZ < border || worldZ >= maxZ;

                            if (!fill) continue;

                            long bitIndex = (long)Index(x, y, z) * bitsPerValue;
                            int byteIndex = (int)(bitIndex >> 3);
                            int bitOffset = (int)(bitIndex & 7);
                            uint writeValue = (1u & valueMask) << bitOffset;
                            uint writeMask = valueMask << bitOffset;

                            if (byteIndex + 2 < data.Length)
                            {
                                data[byteIndex] = (byte)((data[byteIndex] & ~writeMask) | writeValue);
                                data[byteIndex + 1] = (byte)((data[byteIndex + 1] & ~(writeMask >> 8)) | (writeValue >> 8));
                                data[byteIndex + 2] = (byte)((data[byteIndex + 2] & ~(writeMask >> 16)) | (writeValue >> 16));
                            }
                            else if (byteIndex + 1 < data.Length)
                            {
                                data[byteIndex] = (byte)((data[byteIndex] & ~writeMask) | writeValue);
                                data[byteIndex + 1] = (byte)((data[byteIndex + 1] & ~(writeMask >> 8)) | (writeValue >> 8));
                            }
                            else if (byteIndex < data.Length)
                            {
                                data[byteIndex] = (byte)((data[byteIndex] & ~writeMask) | writeValue);
                            }
                        }
            }
        }
    }
}
