using UnityEngine;

namespace World
{
    [System.Serializable]
    public class DesignAreaInjectionStepAsset : WorldDesignPipelineStep
    {
        public DesignArea area;
        public Vector3 offset;

        public override IWorldgenStep CreateStep()
        {
            return new DesignAreaInjectionStep(area, offset);
        }
    }
}
