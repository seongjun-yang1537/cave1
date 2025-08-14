using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace World
{
    public static class BlendMapGaussianBlur
    {
        public static List<Color32> Apply(
            Vector3Int size,
            List<Color32> colors,
            float sigma = 1.0f,
            int iteration = 1,
            int? customRadius = null
        )
        {
            int count = size.x * size.y * size.z;
            if (colors.Count != count) throw new ArgumentException("colors length mismatch");

            var src = new NativeArray<Color32>(colors.ToArray(), Allocator.TempJob);
            var dst = new NativeArray<Color32>(count, Allocator.TempJob);

            var kernel = Build1DKernel(sigma, Allocator.TempJob, customRadius);

            for (int i = 0; i < iteration; ++i)
            {
                new Blur1DJob
                {
                    size = size,
                    axis = 0,
                    kernel = kernel,
                    source = src,
                    result = dst
                }.Schedule(count, 64).Complete();

                Swap(ref src, ref dst);

                new Blur1DJob
                {
                    size = size,
                    axis = 1,
                    kernel = kernel,
                    source = src,
                    result = dst
                }.Schedule(count, 64).Complete();

                Swap(ref src, ref dst);

                new Blur1DJob
                {
                    size = size,
                    axis = 2,
                    kernel = kernel,
                    source = src,
                    result = dst
                }.Schedule(count, 64).Complete();

                Swap(ref src, ref dst);
            }

            var output = new List<Color32>(count);
            output.AddRange(src.ToArray());

            src.Dispose();
            dst.Dispose();
            kernel.Dispose();

            return output;
        }

        [BurstCompile]
        private struct Blur1DJob : IJobParallelFor
        {
            [ReadOnly] public Vector3Int size;
            [ReadOnly] public int axis;
            [ReadOnly] public NativeArray<float> kernel;
            [ReadOnly] public NativeArray<Color32> source;
            [WriteOnly] public NativeArray<Color32> result;

            public void Execute(int index)
            {
                int w = size.x;
                int h = size.y;
                int d = size.z;

                int z = index / (w * h);
                int y = (index / w) % h;
                int x = index % w;

                float4 accum = 0;
                float weightSum = 0;

                int kRadius = kernel.Length >> 1;

                for (int k = -kRadius; k <= kRadius; ++k)
                {
                    int nx = x, ny = y, nz = z;

                    switch (axis)
                    {
                        case 0: nx = math.clamp(x + k, 0, w - 1); break;
                        case 1: ny = math.clamp(y + k, 0, h - 1); break;
                        case 2: nz = math.clamp(z + k, 0, d - 1); break;
                    }

                    int nIdx = nx + ny * w + nz * w * h;
                    Color32 c = source[nIdx];
                    float4 cf = new float4(c.r, c.g, c.b, c.a);
                    float wgt = kernel[k + kRadius];
                    accum += cf * wgt;
                    weightSum += wgt;
                }

                accum /= weightSum;
                float4 rounded = math.round(accum);
                byte r = (byte)math.clamp(rounded.x, 0f, 255f);
                byte g = (byte)math.clamp(rounded.y, 0f, 255f);
                byte b = (byte)math.clamp(rounded.z, 0f, 255f);
                byte a = (byte)math.clamp(rounded.w, 0f, 255f);
                result[index] = new Color32(r, g, b, a);
            }
        }

        private static NativeArray<float> Build1DKernel(
            float sigma,
            Allocator alloc,
            int? customRadius = null
        )
        {
            int radius = customRadius ?? math.max(1, (int)math.ceil(3f * sigma));
            int length = radius * 2 + 1;
            var kernel = new NativeArray<float>(length, alloc);

            float twoSigma2 = 2f * sigma * sigma;
            float sum = 0f;
            for (int i = -radius; i <= radius; ++i)
            {
                float val = math.exp(-(i * i) / twoSigma2);
                kernel[i + radius] = val;
                sum += val;
            }
            for (int i = 0; i < length; ++i) kernel[i] /= sum;
            return kernel;
        }

        private static void Swap<T>(ref NativeArray<T> a, ref NativeArray<T> b) where T : struct
        {
            var tmp = a;
            a = b;
            b = tmp;
        }
    }
}

