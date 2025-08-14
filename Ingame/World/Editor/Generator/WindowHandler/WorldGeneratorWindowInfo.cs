using System;
using Corelib.SUI;
using PathX;
using UnityEngine;

namespace World
{
    [Serializable]
    public class WorldGeneratorWindowInfo
    {
        [NonSerialized]
        private WorldData worldData;

        public WorldGeneratorWindowInfo(WorldData worldData)
        {
            UpdateContext(worldData);
        }

        public SUIElement Render()
        {
            if (worldData == null) return SUIElement.Empty();

            SUIElement content = SUIElement.Empty();

            if (worldData.tree != null)
            {
                content += SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Label($"Tree Nodes: {worldData.tree?.GetNodes()?.Count ?? 0}")
                    + SEditorGUILayout.Label($"Leaf Nodes: {worldData.tree?.GetLeafs()?.Count ?? 0}")
                );
            }

            if (worldData.graph != null)
            {
                content += SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Label($"Graph Nodes: {worldData.graph?.nodeCount ?? 0}")
                    + SEditorGUILayout.Label($"Graph Edges: {worldData.graph?.edgeCount ?? 0}")
                );
            }

            return SEditorGUILayout.Group("Info")
            .Content(
                content
            );
        }

        public void UpdateContext(WorldData worldData)
        {
            this.worldData = worldData;
        }
    }
}