using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using Corelib.SUI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace PathX
{
    public class PathXDebuggerWindow : EditorWindow
    {
        private TriangleDomain selectedDomain = TriangleDomain.All;

        private Vector3 startPoint = Vector3.zero;
        private Vector3 endPoint = Vector3.zero;

        private List<PTriangleGraphNode> visitedNodes;
        private List<PTriangleGraphNode> pathNodes;
        private List<NavSurfacePoint> funnelPath;

        private float pathfindTimeMs = 0f;
        private float pathLength = 0f;

        private bool showVisited = true;
        private bool showPath = true;
        private bool showFunnel = true;
        private bool visible = true;

        [MenuItem("Tools/Game/PathX Debugger")]
        public static void ShowWindow()
        {
            var window = GetWindow<PathXDebuggerWindow>("PathX Debugger");
            window.SetIcon();
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
            SetIcon();
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnGUI()
        {
            if (PathXSystem.Instance == null)
            {
                EditorGUILayout.HelpBox("PathXSystem instance not found in scene.", MessageType.Warning);
                return;
            }

            var domains = PathXSystem.Instance.Engine?.ProfileKeys.ToList();
            if (domains == null) return;

            SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Group("Pathfinding")
                .Content(
                    SEditorGUILayout.Action(() =>
                    {
                        int index = Mathf.Max(0, domains.IndexOf(selectedDomain));
                        string[] names = domains.Select(d => d.ToString()).ToArray();
                        int newIndex = EditorGUILayout.Popup("Profile", index, names);
                        if (newIndex >= 0 && newIndex < domains.Count && newIndex != index)
                        {
                            selectedDomain = domains[newIndex];
                        }
                    })
                    + SEditorGUILayout.Horizontal()
                    .Content(
                        SEditorGUILayout.Var("Start", startPoint).OnValueChanged(v =>
                        {
                            startPoint = v;
                        })
                        + SEditorGUILayout.Button("Focus").OnClick(() => FocusSceneView(startPoint))
                    )
                    + SEditorGUILayout.Horizontal()
                    .Content(
                        SEditorGUILayout.Var("End", endPoint).OnValueChanged(v =>
                        {
                            endPoint = v;
                        })
                        + SEditorGUILayout.Button("Focus").OnClick(() => FocusSceneView(endPoint))
                    )
                    + SEditorGUILayout.Horizontal()
                    .Content(
                        SEditorGUILayout.Button("Pathfind").OnClick(RunPathfinding)
                        + SEditorGUILayout.Button("Clear Result").OnClick(ClearResult)
                    )
                )
                + SEditorGUILayout.Group("Display")
                .Content(
                    SEditorGUILayout.Var("Visible", visible).OnValueChanged(v =>
                    {
                        visible = v;
                        SceneView.RepaintAll();
                        Repaint();
                    })
                    + SEditorGUILayout.Var("Show Visited", showVisited).OnValueChanged(v =>
                    {
                        showVisited = v;
                        SceneView.RepaintAll();
                        Repaint();
                    })
                    + SEditorGUILayout.Var("Show Path Triangles", showPath).OnValueChanged(v =>
                    {
                        showPath = v;
                        SceneView.RepaintAll();
                        Repaint();
                    })
                    + SEditorGUILayout.Var("Show Funnel", showFunnel).OnValueChanged(v =>
                    {
                        showFunnel = v;
                        SceneView.RepaintAll();
                        Repaint();
                    })
                )
                + SEditorGUILayout.Group("Stats")
                .Where(() => pathNodes != null)
                .Content(
                    SEditorGUILayout.Label($"Time (ms): {pathfindTimeMs:F2}")
                    + SEditorGUILayout.Label($"Visited Triangles: {visitedNodes?.Count ?? 0}")
                    + SEditorGUILayout.Label($"Path Length: {pathLength:F2}")
                    + SEditorGUILayout.Label($"Path Triangle Count: {pathNodes?.Count}")
                )
            )
            .Render();
        }

        private void RunPathfinding()
        {
            var navMesh = PathXSystem.GetNavMesh(selectedDomain);
            if (navMesh == null) return;
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
            pathNodes = AstarDebug.FindTrianglePathWithVisited(
                navMesh.Graph,
                navMesh.ChunkIndex,
                startPoint,
                endPoint,
                PathfindingSettings.Default,
                out visitedNodes);
            sw.Stop();
            pathfindTimeMs = (float)sw.Elapsed.TotalMilliseconds;
            funnelPath = FunnelAlgorithm.FindPathCorners(startPoint, endPoint, pathNodes);
            pathLength = 0f;
            if (funnelPath != null)
            {
                for (int i = 0; i < funnelPath.Count - 1; i++)
                {
                    pathLength += Vector3.Distance(funnelPath[i].point, funnelPath[i + 1].point);
                }
            }
            SceneView.RepaintAll();
        }

        private void ClearResult()
        {
            visitedNodes = null;
            pathNodes = null;
            funnelPath = null;
            pathfindTimeMs = 0f;
            pathLength = 0f;
            SceneView.RepaintAll();
        }

        private static void FocusSceneView(Vector3 position)
        {
            if (SceneView.lastActiveSceneView != null)
            {
                SceneView.lastActiveSceneView.LookAt(position, SceneView.lastActiveSceneView.rotation, 5f);
            }
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (!visible) return;
            Color prevColor = Handles.color;
            EditorGUI.BeginChangeCheck();
            Handles.color = Color.green;
            startPoint = Handles.PositionHandle(startPoint, Quaternion.identity);
            Handles.Label(startPoint + Vector3.up * 0.1f, "Start");
            Handles.color = Color.red;
            endPoint = Handles.PositionHandle(endPoint, Quaternion.identity);
            Handles.Label(endPoint + Vector3.up * 0.1f, "End");
            if (EditorGUI.EndChangeCheck())
            {
                Repaint();
            }

            if (PathXSystem.Instance == null) return;
            var navMesh = PathXSystem.GetNavMesh(selectedDomain);
            if (navMesh != null)
            {
                Vector3 startProj = navMesh.PointLocation(startPoint);
                Vector3 endProj = navMesh.PointLocation(endPoint);
                PTriangle startTri = PathXSystem.TriangleLocation(selectedDomain, startPoint);
                PTriangle endTri = PathXSystem.TriangleLocation(selectedDomain, endPoint);

                Handles.color = Color.red;
                Handles.DrawDottedLine(startPoint, startProj, 4f);
                Handles.DrawDottedLine(endPoint, endProj, 4f);
                Handles.SphereHandleCap(0, startProj, Quaternion.identity, 0.1f, EventType.Repaint);
                Handles.SphereHandleCap(0, endProj, Quaternion.identity, 0.1f, EventType.Repaint);

                if (startTri != null)
                {
                    Handles.color = new Color(0f, 1f, 0f, 0.15f);
                    Handles.DrawAAConvexPolygon(startTri.v0, startTri.v1, startTri.v2);
                }

                if (endTri != null)
                {
                    Handles.color = new Color(1f, 0f, 0f, 0.15f);
                    Handles.DrawAAConvexPolygon(endTri.v0, endTri.v1, endTri.v2);
                }
            }

            if (visitedNodes != null && showVisited)
            {
                Handles.color = new Color(1f, 0f, 0f, 0.2f);
                foreach (var node in visitedNodes)
                {
                    Handles.DrawAAConvexPolygon(node.v0, node.v1, node.v2);
                }
            }

            if (pathNodes != null && showPath)
            {
                Handles.color = Color.green;
                foreach (var node in pathNodes)
                {
                    Handles.DrawAAPolyLine(2f, node.v0, node.v1, node.v2, node.v0);
                }
            }

            if (funnelPath != null && showFunnel)
            {
                Handles.color = Color.cyan;
                for (int i = 0; i < funnelPath.Count - 1; i++)
                {
                    Handles.DrawLine(funnelPath[i].point, funnelPath[i + 1].point);
                }
            }
            Handles.color = prevColor;
        }

        private void SetIcon()
        {
            string iconName = EditorGUIUtility.isProSkin ? "d_UnityEditor.ConsoleWindow" : "UnityEditor.ConsoleWindow";
            titleContent.image = EditorGUIUtility.IconContent(iconName).image;
        }
    }
}
