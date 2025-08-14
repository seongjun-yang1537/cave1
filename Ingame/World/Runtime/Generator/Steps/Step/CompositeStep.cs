using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Corelib.Utils;
using VoxelEngine;

namespace World
{
    public class CompositeStep : IWorldgenStep
    {
        private readonly List<IWorldgenStep> steps;

        public CompositeStep(IEnumerable<IWorldgenStep> steps)
        {
            this.steps = new List<IWorldgenStep>(steps);
        }

        public async UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            foreach (var step in steps)
            {
                if (step == null) continue;
                await step.ExecuteAsync(rng, worldData, worldMap);
            }
        }
    }
}
