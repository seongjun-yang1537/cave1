using System;

namespace World
{
    [Serializable]
    public abstract class WorldGraphPipelineStep : WorldPipelineStep { }
    [Serializable]
    public abstract class WorldRasterizePipelineStep : WorldPipelineStep { }
    [Serializable]
    public abstract class WorldPopulatePipelineStep : WorldPipelineStep { }
    [Serializable]
    public abstract class WorldDesignPipelineStep : WorldPipelineStep { }
}
