using System;
using System.Collections.Generic;
using Corelib.SUI;
using Corelib.Utils;
using UnityEditor;
using UnityEngine;

namespace World
{
    [Serializable]
    public class WorldGeneratorWindowStartEnd
    {
        [NonSerialized] private TriangleVisualizer triangleVisualizer;
        [NonSerialized] private WorldData worldData;
        [SerializeField] private bool showPositions = false;

        public WorldGeneratorWindowStartEnd(GameWorld gameWorld)
        {
            UpdateContext(gameWorld);
        }

        public SUIElement Render()
        {
            if (triangleVisualizer == null || worldData == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical("box")
            .Content(
                SEditorGUILayout.Label("Start/End")
                    .Bold()
                    .Align(TextAnchor.MiddleCenter)
                    + SEditorGUILayout.Var("Show", showPositions)
                        .OnValueChanged(v => { showPositions = v; RenderPositions(); })
                    + SEditorGUILayout.Button("Focus Start").OnClick(FocusStart)
                    + SEditorGUILayout.Button("Focus End").OnClick(FocusEnd)
            );
        }

        private void RenderPositions()
        {
            if (triangleVisualizer == null) return;
            triangleVisualizer.meshVisualizer.HideMesh("StartVoxel");
            triangleVisualizer.meshVisualizer.HideMesh("EndVoxel");
            if (!showPositions) return;

            var mesh = CreateCubeMesh(Vector3.one);
            if (mesh != null)
            {
                GameObject go = triangleVisualizer.meshVisualizer.ShowMesh("StartVoxel", mesh);
                if (go != null)
                    go.transform.position = (Vector3)worldData.startVoxel + Vector3.one * 0.5f;

                go = triangleVisualizer.meshVisualizer.ShowMesh("EndVoxel", mesh);
                if (go != null)
                    go.transform.position = (Vector3)worldData.endVoxel + Vector3.one * 0.5f;
            }
        }

        private void FocusStart()
        {
            Debug.Log(worldData.startVoxel);
            FocusVoxel(worldData.startVoxel);
        }
        private void FocusEnd()
        {
            Debug.Log(worldData.endVoxel);
            FocusVoxel(worldData.endVoxel);
        }

        private static void FocusVoxel(Vector3Int voxel)
        {
            var center = (Vector3)voxel + Vector3.one * 0.5f;
            var bounds = new Bounds(center, Vector3.one * 2f);
            SceneView.lastActiveSceneView?.Frame(bounds, false);
        }

        private static Mesh CreateCubeMesh(Vector3 size)
        {
            Vector3 hs = size * 0.5f;
            Vector3[] vertices =
            {
                new(-hs.x, -hs.y, -hs.z),
                new(hs.x, -hs.y, -hs.z),
                new(hs.x, -hs.y, hs.z),
                new(-hs.x, -hs.y, hs.z),
                new(-hs.x, hs.y, -hs.z),
                new(hs.x, hs.y, -hs.z),
                new(hs.x, hs.y, hs.z),
                new(-hs.x, hs.y, hs.z)
            };
            int[] triangles =
            {
                0,2,1,0,3,2,
                4,5,6,4,6,7,
                4,7,3,4,3,0,
                1,2,6,1,6,5,
                3,7,6,3,6,2,
                4,0,1,4,1,5
            };
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            return mesh;
        }

        public void UpdateContext(GameWorld gameWorld)
        {
            if (gameWorld == null)
            {
                triangleVisualizer = null;
                worldData = null;
                RenderPositions();
                return;
            }

            triangleVisualizer = gameWorld.GetComponentInChildren<TriangleVisualizer>();
            worldData = gameWorld.worldData;
            RenderPositions();
        }
    }
}
