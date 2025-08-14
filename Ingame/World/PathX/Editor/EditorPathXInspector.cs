// using System.Collections.Generic;
// using System.Linq;
// using Corelib.SUI;
// using UnityEditor;
// using UnityEngine;

// namespace PathX
// {
//     [CustomEditor(typeof(PathXInspector))]
//     public class EditorPathXInspector : Editor
//     {
//         PathXInspector script;
//         PathXInspectorEditorData editorData { get => script.editorData; }
//         protected void OnEnable()
//         {
//             script = (PathXInspector)target;
//         }

//         public override void OnInspectorGUI()
//         {
//             SEditorGUILayout.Vertical()
//             .Content(
//                 SEditorGUILayout.Label($"{(script.engine == null ? "off" : "on")}")
//                 + PathXEngineInspector.Render(script.engine)
//                 + RenderPathXInspector(script)
//             )
//             .Render();

//             if (editorData.enableChunkIndexHandle || editorData.enablePointLocationHandle)
//             {
//                 SceneView.RepaintAll();
//             }
//         }

//         private SUIElement RenderPathXInspector(PathXInspector inspector)
//         {
//             return SEditorGUILayout.Vertical()
//             .Content(
//                 SEditorGUILayout.Object("MeshFilter", inspector.meshFilter, typeof(MeshFilter))
//                 .OnValueChanged(value => inspector.meshFilter = (MeshFilter)value)
//                 + SEditorGUILayout.Int("Chunk Size", inspector.chunkSize)
//                 .OnValueChanged(value => inspector.chunkSize = value)
//                 + SEditorGUILayout.Horizontal()
//                 .Content(
//                     SEditorGUILayout.Button("Reload")
//                     .OnClick(() =>
//                     {
//                         inspector.ReloadEngine(inspector.meshFilter.sharedMesh);
//                     })
//                     + SEditorGUILayout.Button("Hide")
//                     .OnClick(() =>
//                     {
//                         inspector.HideDebugMesh();
//                     })
//                 )
//                 + RenderChunkIndex(inspector)
//                 + RenderPointLocation(inspector)
//                 + RenderPathfind(inspector)
//             );
//         }

//         private SUIElement RenderPathfind(PathXInspector inspector)
//         {
//             if (inspector.engine == null) return SUIElement.Empty();
//             ChunkGeometryIndex<PTriangle> chunkIndex = inspector.engine.chunkIndex;
//             ChunkGeometryData<PTriangle> chunkData = chunkIndex?[editorData.selectedChunkIndex];

//             return SEditorGUILayout.Group("Path Find")
//             .Content(
//                 SEditorGUILayout.Toggle("Enable Pathfind Handle", editorData.enablePathfindHandle)
//                 .OnValueChanged(value => editorData.enablePathfindHandle = value)
//                 + SEditorGUILayout.Vertical()
//                 .Where(() => editorData.enablePathfindHandle)
//                 .Content(
//                     SEditorGUILayout.Vector3("Start Position", editorData.pathfindHandlePositionStart)
//                     .OnValueChanged(value => editorData.pathfindHandlePositionStart = value)
//                     + SEditorGUILayout.Vector3("End Position", editorData.pathfindHandlePositionEnd)
//                     .OnValueChanged(value => editorData.pathfindHandlePositionEnd = value)

//                     + SEditorGUILayout.Button("Pathfind")
//                     .OnClick(() =>
//                     {
//                         List<PTriangleGraphNode> path = Astar.FindTriangleChannel(
//                             inspector.engine,
//                             editorData.pathfindHandlePositionStart,
//                             editorData.pathfindHandlePositionEnd
//                         );
//                         editorData.pathfindNodes = path;

//                         List<NavPortal> portals = FunnelAlgorithm.ExtractNavPortal(path);

//                         editorData.pathfindPortals = portals;

//                         List<NavSurfacePoint> points = FunnelAlgorithm.FindPathCorners(
//                             editorData.pathfindHandlePositionStart,
//                             editorData.pathfindHandlePositionEnd,
//                             path
//                         );

//                         editorData.pathfindLine = points.Select(p => p.point).ToList();
//                         editorData.pathfindNormals = points.Select(p => p.normal).ToList();
//                         SceneView.RepaintAll();
//                     })
//                 )
//             );
//         }

//         private SUIElement RenderPointLocation(PathXInspector inspector)
//         {
//             if (inspector.engine == null) return SUIElement.Empty();
//             ChunkGeometryIndex<PTriangle> chunkIndex = inspector.engine.chunkIndex;
//             ChunkGeometryData<PTriangle> chunkData = chunkIndex?[editorData.selectedChunkIndex];

//             return SEditorGUILayout.Group("Point Location")
//             .Content(
//                 SEditorGUILayout.Toggle("Enable Point Location Handle", editorData.enablePointLocationHandle)
//                 .OnValueChanged(value => editorData.enablePointLocationHandle = value)
//                 + SEditorGUILayout.Vertical()
//                 .Where(() => editorData.enablePointLocationHandle)
//                 .Content(
//                     SEditorGUILayout.Vector3("Handle Position", editorData.pointLocationHandlePosition)
//                     .OnValueChanged(value => editorData.pointLocationHandlePosition = value)
//                 )
//             );
//         }

//         private SUIElement RenderChunkIndex(PathXInspector inspector)
//         {
//             if (inspector.engine == null) return SUIElement.Empty();
//             ChunkGeometryIndex<PTriangle> chunkIndex = inspector.engine.chunkIndex;
//             ChunkGeometryData<PTriangle> chunkData = chunkIndex?[editorData.selectedChunkIndex];

//             return SEditorGUILayout.Group("Chunk Index Debug")
//             .Content(
//                 SEditorGUILayout.Toggle("Enable Chunk Index Handle", editorData.enableChunkIndexHandle)
//                 .OnValueChanged(value =>
//                 {
//                     editorData.enableChunkIndexHandle = value;
//                     if (value)
//                     {
//                         ShowMeshFromChunkIndexHandle(editorData.selectedChunkIndex);
//                     }
//                     else
//                     {
//                         inspector.meshVisualizer.HideMesh("Triangle Wire Mesh");
//                         inspector.meshVisualizer.HideMesh("Triangle Graph Mesh");
//                     }
//                 })
//                 + SEditorGUILayout.Vertical()
//                 .Where(() => editorData.enableChunkIndexHandle)
//                 .Content(
//                     SEditorGUILayout.Vector3("Handle Position", editorData.chunkIndexHandlePosition)
//                     .OnValueChanged(value => editorData.chunkIndexHandlePosition = value)

//                     + SEditorGUILayout.Vector3Int("Selected Chunk Index", editorData.selectedChunkIndex)
//                     .OnValueChanged(value => editorData.selectedChunkIndex = value)
//                 )
//             );
//         }

//         private void OnSceneGUI()
//         {
//             RenderChunkIndexDebugHandle();
//             RenderPointLocationDebugHandle();
//             RenderPathfindHandle();
//         }

//         private void ShowMeshFromChunkIndexHandle(Vector3Int selectedChunkIndex)
//         {
//             var chunkIndex = script.engine.chunkIndex;
//             if (chunkIndex == null) return;

//             ChunkGeometryData<PTriangle> chunkData = chunkIndex?[selectedChunkIndex];
//             if (chunkData == null) script.HideDebugMesh();
//             else script.ShowDebugMesh(chunkData.triangles);
//         }

//         private void RenderPathfindHandle()
//         {
//             if (!editorData.enablePathfindHandle) return;

//             if (script == null || script.engine == null) return;

//             var chunkIndex = script.engine.chunkIndex;
//             if (chunkIndex == null) return;

//             EditorGUI.BeginChangeCheck();
//             editorData.pathfindHandlePositionStart = Handles.PositionHandle(editorData.pathfindHandlePositionStart, Quaternion.identity);
//             editorData.pathfindHandlePositionEnd = Handles.PositionHandle(editorData.pathfindHandlePositionEnd, Quaternion.identity);
//             if (EditorGUI.EndChangeCheck())
//             {
//                 SceneView.RepaintAll();
//             }

//             RenderClosestPointOnTriangle(editorData.pathfindHandlePositionStart);
//             RenderClosestPointOnTriangle(editorData.pathfindHandlePositionEnd);

//             RenderHandleSphere(editorData.pathfindHandlePositionStart);
//             RenderHandleSphere(editorData.pathfindHandlePositionEnd);
//         }

//         private void RenderChunkIndexDebugHandle()
//         {
//             if (!editorData.enableChunkIndexHandle) return;

//             if (script == null || script.engine == null) return;

//             var chunkIndex = script.engine.chunkIndex;
//             if (chunkIndex == null) return;

//             EditorGUI.BeginChangeCheck();
//             editorData.chunkIndexHandlePosition = Handles.PositionHandle(editorData.chunkIndexHandlePosition, Quaternion.identity);
//             if (EditorGUI.EndChangeCheck())
//             {
//                 SceneView.RepaintAll();
//             }

//             RenderHandleSphere(editorData.chunkIndexHandlePosition);

//             Vector3Int prevChunkIndex = editorData.selectedChunkIndex;
//             editorData.selectedChunkIndex = chunkIndex.GetChunkIndexFromPoint(editorData.chunkIndexHandlePosition);
//             if (editorData.selectedChunkIndex != prevChunkIndex)
//             {
//                 ShowMeshFromChunkIndexHandle(editorData.selectedChunkIndex);
//             }
//         }

//         private void RenderPointLocationDebugHandle()
//         {
//             if (!editorData.enablePointLocationHandle) return;

//             if (script == null || script.engine == null) return;

//             var chunkIndex = script.engine.chunkIndex;
//             if (chunkIndex == null) return;

//             EditorGUI.BeginChangeCheck();
//             editorData.pointLocationHandlePosition = Handles.PositionHandle(editorData.pointLocationHandlePosition, Quaternion.identity);
//             if (EditorGUI.EndChangeCheck())
//             {
//                 SceneView.RepaintAll();
//             }

//             RenderClosestPointOnTriangle(editorData.pointLocationHandlePosition);
//             RenderHandleSphere(editorData.pointLocationHandlePosition);
//         }

//         private void RenderClosestPointOnTriangle(Vector3 point)
//         {
//             var chunkIndex = script.engine.chunkIndex;
//             if (chunkIndex == null) return;

//             var closestTriangle = chunkIndex.QueryPoint(point);
//             if (closestTriangle != null)
//             {
//                 var (v0, v1, v2) = (closestTriangle.v0, closestTriangle.v1, closestTriangle.v2);

//                 Handles.color = Color.yellow;
//                 Handles.DrawLine(v0, v1, 2);
//                 Handles.DrawLine(v1, v2, 2);
//                 Handles.DrawLine(v2, v0, 2);

//                 var closestPointOnSurface = closestTriangle.ClosestPointOnTriangle(point);

//                 Handles.color = Color.red;
//                 Handles.DrawLine(point, (Vector3)closestPointOnSurface, 2);

//                 Handles.SphereHandleCap(0, (Vector3)closestPointOnSurface, Quaternion.identity, 0.1f, EventType.Repaint);
//             }
//         }

//         private void RenderHandleSphere(Vector3 position)
//         {
//             Handles.color = Color.yellow;
//             Handles.SphereHandleCap(0, position, Quaternion.identity, 0.25f, EventType.Repaint);
//         }
//     }
// }