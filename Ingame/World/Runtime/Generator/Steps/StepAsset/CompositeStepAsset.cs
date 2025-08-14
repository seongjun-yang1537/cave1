using System.Collections.Generic;
using UnityEngine;

namespace World
{
    [System.Serializable]
    public class CompositeStepAsset : WorldPipelineStep
    {
        [SerializeReference]
        public List<WorldPipelineStep> steps = new();

        public override IWorldgenStep CreateStep()
        {
            var built = new List<IWorldgenStep>();
            foreach (var step in steps)
            {
                if (step == null) continue;
                built.Add(step.CreateStep());
            }
            return new CompositeStep(built);
        }
    }
}
