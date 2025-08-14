using Cysharp.Threading.Tasks;
using Corelib.Utils;
using UnityEngine;
using VoxelEngine;

namespace World
{
    public class SmoothFieldStep : IWorldgenRasterizeStep
    {
        private readonly float[] kernel;

        public SmoothFieldStep()
        {
            // simple 3x3x3 averaging kernel
            kernel = new float[27];
            for (int i = 0; i < kernel.Length; i++)
                kernel[i] = 1f / 27f;
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            var field = worldData?.chunkedScalarField;
            if (field == null) return UniTask.CompletedTask;

            ScalarFieldConvolver.Convolve(field, kernel);
            return UniTask.CompletedTask;
        }
    }
}
