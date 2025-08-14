using UnityEditor;
using UnityEditor.Callbacks;
using World;

/// <summary>
/// Opens <see cref="WorldgenGraphWindow"/> when a <see cref="WorldgenStepGraph"/> asset is double clicked.
/// </summary>
public static class WorldgenGraphAssetCallbacks
{
    [OnOpenAsset]
    public static bool OnOpenGraph(int instanceID, int line)
    {
        var asset = EditorUtility.InstanceIDToObject(instanceID) as WorldgenStepGraph;
        if (asset != null)
        {
            // Create a new window instance for each opened graph so multiple
            // graphs can be edited independently, similar to ShaderGraph.
            var window = EditorWindow.CreateWindow<WorldgenGraphWindow>();
            window.InitializeGraph(asset);
            window.Show();
            return true;
        }

        return false;
    }
}
