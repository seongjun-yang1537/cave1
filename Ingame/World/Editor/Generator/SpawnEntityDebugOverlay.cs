using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Corelib.Utils;
using Ingame;
using PathX;
using Core;
using Cysharp.Threading.Tasks;

namespace World
{
    public class SpawnEntityDebugOverlay : System.IDisposable
    {
        public WorldData worldData;
        public TriangleVisualizer triangleVisualizer;

        public EntityType entityType = EntityType.None;
        public float poissonRadius = 3f;
        public int maxCount = 10;
        public float spawnProbability = 1f;
        public float triangleRadius = 1f;
        public TriangleDomain domain = TriangleDomain.Ground0;
        public BiomeFlags biomeMask = BiomeFlags.All;
        public int clusterCount = 1;
        public float clusterRadius = 0f;
        public int neighborDepth = 0;

        private Vector3 center = Vector3.zero;
        private readonly SphereBoundsHandle handle = new();
        private readonly Rect overlayRect = new(10, 20, 260f, 260f);
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
                {
                    SceneView.duringSceneGui -= OnSceneGUI;
                    triangleVisualizer?.meshVisualizer.HideMesh("SpawnPreview");
                }
            }
        }

        public void Dispose() => Visible = false;

        private void OnSceneGUI(SceneView sceneView)
        {
            if (worldData == null) return;

            Handles.color = Color.green;
            EditorGUI.BeginChangeCheck();
            center = Handles.PositionHandle(center, Quaternion.identity);
            handle.center = center;
            handle.radius = triangleRadius;
            handle.DrawHandle();
            if (EditorGUI.EndChangeCheck())
            {
                center = SnapVector(handle.center);
                SceneView.RepaintAll();
            }

            Handles.BeginGUI();
            if (overlayStyle == null)
                overlayStyle = new GUIStyle(GUI.skin.window);

            Rect windowRect = new(overlayRect);
            windowRect.y = sceneView.position.height - (overlayRect.y + overlayRect.height);

            GUI.Box(windowRect, GUIContent.none, overlayStyle);
            GUI.Window(192837, windowRect, DrawOverlay, GUIContent.none, GUIStyle.none);
            Handles.EndGUI();
        }

        private void DrawOverlay(int id)
        {
            entityType = (EntityType)EditorGUILayout.EnumPopup("Entity Type", entityType);
            poissonRadius = EditorGUILayout.FloatField("Poisson Radius", poissonRadius);
            maxCount = EditorGUILayout.IntField("Max Count", maxCount);
            spawnProbability = EditorGUILayout.Slider("Spawn Prob", spawnProbability, 0f, 1f);
            triangleRadius = EditorGUILayout.FloatField("Triangle Radius", triangleRadius);
            domain = (TriangleDomain)EditorGUILayout.EnumPopup("Domain", domain);
            biomeMask = (BiomeFlags)EditorGUILayout.EnumFlagsField("Biome Mask", biomeMask);
            neighborDepth = EditorGUILayout.IntField("Neighbor Depth", neighborDepth);
            clusterCount = EditorGUILayout.IntField("Cluster Count", clusterCount);
            clusterRadius = EditorGUILayout.FloatField("Cluster Radius", clusterRadius);

            if (GUILayout.Button("Apply", GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                Apply().Forget();

            GUI.DragWindow(new Rect(0, 0, overlayRect.width, 20));
        }

        private async UniTaskVoid Apply()
        {
            if (worldData == null) return;

            var rng = GameRng.World;

            var step = new SpawnEntityStep(
                entityType,
                poissonRadius,
                maxCount,
                spawnProbability,
                triangleRadius,
                domain,
                biomeMask,
                clusterCount,
                clusterRadius,
                neighborDepth);

            var (spawnInfos, previewTris) = await step.PreviewAsync(rng, worldData, center);

            if (triangleVisualizer != null)
            {
                triangleVisualizer.meshVisualizer.HideMesh("SpawnPreview");
                if (previewTris.Count > 0)
                {
                    var mesh = MeshGenerator.CreateMeshFromTriangles(previewTris);
                    triangleVisualizer.meshVisualizer.ShowMesh("SpawnPreview", mesh);
                }
            }

            foreach (var info in spawnInfos)
            {
                var controller = EntitySystem.SpawnEntity(entityType);
                controller.transform.position = info.pos;
                controller.Spawn();
                controller.SnapToNavMesh();
            }
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
