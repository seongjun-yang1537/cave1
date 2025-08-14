using System.Collections.Generic;
using UnityEngine;

namespace World
{
    [CreateAssetMenu(menuName = "Game/World/Step Graph/Populate", fileName = "Populate Step Graph")]
    public class PopulateStepGraph : WorldgenStepGraph
    {
        [SerializeReference]
        public List<WorldPipelineStep> steps = new();

        public void RefreshSteps()
        {
            steps.Clear();
            foreach (var step in EnumerateSteps())
            {
                steps.Add(step);
            }
        }
    }
}
