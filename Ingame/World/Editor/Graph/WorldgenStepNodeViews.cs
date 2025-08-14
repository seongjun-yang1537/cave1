using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using GraphProcessor;
using World.Common;

[NodeCustomEditor(typeof(WorldgenStepStartNode))]
public class WorldgenStepStartNodeView : BaseNodeView
{
    public override void Enable()
    {
        // Clear default title so we can customize it
        title = "Start";
        var icon = new Image
        {
            image = EditorGUIUtility.IconContent("d_PlayButton On").image,
            scaleMode = ScaleMode.ScaleToFit
        };
        icon.style.width = 48;
        icon.style.height = 48;
        titleContainer.Insert(0, icon);
        SetNodeColor(new Color(0.3f, 0.8f, 0.3f));
    }
}

[NodeCustomEditor(typeof(WorldgenStepEndNode))]
public class WorldgenStepEndNodeView : BaseNodeView
{
    public override void Enable()
    {
        title = "End";
        var icon = new Image
        {
            image = EditorGUIUtility.IconContent("d_PauseButton On").image,
            scaleMode = ScaleMode.ScaleToFit
        };
        icon.style.width = 48;
        icon.style.height = 48;
        titleContainer.Insert(0, icon);
        SetNodeColor(new Color(0.8f, 0.3f, 0.3f));
    }
}

[NodeCustomEditor(typeof(World.GenerateVSPTreeStepNode))]
public class GenerateVSPTreeStepNodeView : BaseNodeView
{
    public override void Enable()
    {
        base.Enable();
        SetNodeColor(new Color(1f, 0.85f, 0.2f));
    }
}

[NodeCustomEditor(typeof(World.GenerateRoomsStepNode))]
public class GenerateRoomsStepNodeView : BaseNodeView
{
    public override void Enable()
    {
        base.Enable();
        SetNodeColor(new Color(1f, 0.85f, 0.2f));
    }
}

[NodeCustomEditor(typeof(World.RenderWorldStepNode))]
public class RenderWorldStepNodeView : BaseNodeView
{
    public override void Enable()
    {
        base.Enable();
        SetNodeColor(new Color(1f, 0.85f, 0.2f));
    }
}

[NodeCustomEditor(typeof(World.GenerateTriangleLayerStepNode))]
public class GenerateTriangleLayerStepNodeView : BaseNodeView
{
    public override void Enable()
    {
        base.Enable();
        SetNodeColor(new Color(1f, 0.85f, 0.2f));
    }
}

[NodeCustomEditor(typeof(World.GeneratePathXNavMeshStepNode))]
public class GeneratePathXNavMeshStepNodeView : BaseNodeView
{
    public override void Enable()
    {
        base.Enable();
        SetNodeColor(new Color(1f, 0.85f, 0.2f));
    }
}
