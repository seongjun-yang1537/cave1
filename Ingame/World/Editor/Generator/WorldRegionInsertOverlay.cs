using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace World
{
    public class WorldRegionInsertOverlay : System.IDisposable
    {
        public WorldVSPGraph graph;
        public WorldVSPGraphVisualizer visualizer;

        private Vector3 regionCenter = Vector3.zero;
        private Vector3 regionSize = Vector3.one * 4f;
        private BiomeType regionBiome = BiomeType.Hollow;
        private readonly BoxBoundsHandle regionHandle = new();
        private readonly Rect overlayRect = new(10, 20, 200f, 130f);
        private GUIStyle overlayStyle;

        private bool visible;
        public bool Visible
        {
            get => visible;
            set
            {
                if (visible == value) return;
                visible = value;
                if (visible)
                    SceneView.duringSceneGui += OnSceneGUI;
                else
                    SceneView.duringSceneGui -= OnSceneGUI;
            }
        }

        public void Dispose() => Visible = false;

        private void OnSceneGUI(SceneView sceneView)
        {
            if (graph == null) return;

            Handles.color = GetBiomeColor(regionBiome);
            EditorGUI.BeginChangeCheck();
            regionCenter = Handles.PositionHandle(regionCenter, Quaternion.identity);
            regionHandle.center = regionCenter;
            regionHandle.size = regionSize;
            regionHandle.DrawHandle();
            if (EditorGUI.EndChangeCheck())
            {
                regionCenter = SnapVector(regionHandle.center);
                regionSize = SnapVector(regionHandle.size);
                SceneView.RepaintAll();
            }

            Handles.BeginGUI();
            if (overlayStyle == null)
                overlayStyle = new GUIStyle(GUI.skin.window);

            Rect windowRect = new(overlayRect);
            windowRect.y = sceneView.position.height - (overlayRect.y + overlayRect.height);

            GUI.Box(windowRect, GUIContent.none, overlayStyle);
            GUI.Window(132245, windowRect, DrawOverlay, GUIContent.none, GUIStyle.none);
            Handles.EndGUI();
        }

        private void DrawOverlay(int id)
        {
            regionCenter = EditorGUILayout.Vector3Field("Center", regionCenter);
            regionSize = EditorGUILayout.Vector3Field("Size", regionSize);
            regionBiome = (BiomeType)EditorGUILayout.EnumPopup("Biome", regionBiome);
            if (GUILayout.Button("Insert", GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                InsertRegion();
            GUI.DragWindow(new Rect(0, 0, overlayRect.width, 20));
        }

        private void InsertRegion()
        {
            if (graph == null) return;

            Vector3Int iSize = Vector3Int.Max(Vector3Int.one, Vector3Int.RoundToInt(regionSize));
            Vector3Int tl = Vector3Int.RoundToInt(regionCenter - (Vector3)iSize * 0.5f);
            Vector3Int br = tl + iSize;
            var region = new WorldVSPGraphNode(tl, br) { biome = regionBiome };
            graph.InsertRegion(region);
            if (visualizer != null)
            {
                visualizer.SetGraph(graph);
                visualizer.Render();
            }
            SceneView.RepaintAll();
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

        private static Vector3 SnapVector(Vector3 value)
        {
            return new Vector3(
                Mathf.Round(value.x),
                Mathf.Round(value.y),
                Mathf.Round(value.z)
            );
        }
    }
}
