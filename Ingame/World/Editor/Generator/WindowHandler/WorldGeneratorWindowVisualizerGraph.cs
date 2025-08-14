using System;
using Corelib.SUI;
using UnityEditor;
using UnityEngine;

namespace World
{
    [Serializable]
    public class WorldGeneratorWindowVisualizerGraph
    {
        [NonSerialized]
        private WorldVSPGraphVisualizer graphVisualizer;

        [SerializeField]
        private bool foldBiomeFilters = true;

        public WorldGeneratorWindowVisualizerGraph(GameWorld gameWorld)
        {
            UpdateContext(gameWorld);
        }

        public SUIElement Render()
        {
            if (graphVisualizer == null) return SUIElement.Empty();

            return SEditorGUILayout.Group("Visualizer")
            .Content(
                SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Label("Graph")
                    .Bold()
                    .Align(TextAnchor.MiddleCenter)
                    + SEditorGUILayout.Separator()
                    + SEditorGUILayout.Var("Show Graph Nodes", graphVisualizer.showGraphNodes)
                    .OnValueChanged(v => graphVisualizer.showGraphNodes = v)
                    + SEditorGUILayout.Var("Show Graph Edges", graphVisualizer.showGraphEdges)
                    .OnValueChanged(v => graphVisualizer.showGraphEdges = v)
                    + SEditorGUILayout.Separator()

                    + SEditorGUILayout.Label("Tree")
                    .Bold()
                    .Align(TextAnchor.MiddleCenter)
                    + SEditorGUILayout.Separator()
                    + SEditorGUILayout.Var("Show Tree Nodes", graphVisualizer.showTreeNodes)
                    .OnValueChanged(v => graphVisualizer.showTreeNodes = v)
                    + SEditorGUILayout.Var("Show Tree Edges", graphVisualizer.showTreeEdges)
                    .OnValueChanged(v => graphVisualizer.showTreeEdges = v)
                    + SEditorGUILayout.Separator()

                    + SEditorGUILayout.Label("Biome")
                    .Bold()
                    .Align(TextAnchor.MiddleCenter)
                    + SEditorGUILayout.Separator()
                    + SEditorGUILayout.Var("Show Biomes", graphVisualizer.showBiomes)
                    .OnValueChanged(v => graphVisualizer.showBiomes = v)
                    + SEditorGUILayout.Var("Show Rooms", graphVisualizer.showRooms)
                    .OnValueChanged(v => graphVisualizer.showRooms = v)
                    + SEditorGUILayout.FoldGroup("Biome Filters", foldBiomeFilters)
                        .OnValueChanged(v => foldBiomeFilters = v)
                        .Content(
                            RenderBiomeToggles()
                        )
                    + SEditorGUILayout.Button("Update")
                    .OnClick(() => graphVisualizer?.Render())
                    + SEditorGUILayout.Separator()
                )
            );
        }

        private SUIElement RenderBiomeToggles()
        {
            SUIElement elements = SUIElement.Empty();
            if (graphVisualizer == null) return elements;

            foreach (BiomeType biome in Enum.GetValues(typeof(BiomeType)))
            {
                if (biome == BiomeType.None) continue;
                bool visible = graphVisualizer.IsBiomeVisible(biome);
                elements += SEditorGUILayout.Horizontal()
                    .Content(
                        SEditorGUILayout.Var(biome.ToString(), visible)
                            .OnValueChanged(v => graphVisualizer.SetBiomeVisible(biome, v))
                        + SEditorGUILayout.Action(() =>
                        {
                            Rect r = GUILayoutUtility.GetRect(16f, EditorGUIUtility.singleLineHeight);
                            r.width = 16f;
                            EditorGUI.DrawRect(r, GetBiomeColor(biome));
                        })
                    );
            }
            return elements;
        }

        private static Color GetBiomeColor(BiomeType biome)
        {
            return biome switch
            {
                BiomeType.Hollow => new Color(0.6f, 0.6f, 0.6f),
                BiomeType.Threadway => Color.yellow,
                BiomeType.Behemire => Color.red,
                BiomeType.Veinreach => Color.blue,
                BiomeType.Fangspire => Color.cyan,
                BiomeType.Silken => Color.magenta,
                _ => Color.gray,
            };
        }

        public void UpdateContext(GameWorld gameWorld)
        {
            if (gameWorld == null)
            {
                graphVisualizer = null;
                return;
            }

            graphVisualizer = gameWorld.GetComponentInChildren<WorldVSPGraphVisualizer>();
        }
    }
}