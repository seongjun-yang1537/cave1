using UnityEngine;

namespace World
{
    [System.Serializable]
    public class PaintBiomesStepAsset : WorldGraphPipelineStep
    {
        [SerializeField]
        public PaintBiomesConfig config;

        public override IWorldgenStep CreateStep()
        {
            return new PaintBiomesStep(config);
        }
    }
}
