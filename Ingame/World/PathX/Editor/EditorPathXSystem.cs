using Corelib.SUI;
using Corelib.Utils;
using UnityEditor;
using UnityEngine;

namespace PathX
{
    [CustomEditor(typeof(PathXSystem))]
    public class EditorPathXSystem : Editor
    {
        PathXSystem script;
        MeshVisualizer meshVisualizer;
        protected void OnEnable()
        {
            script = (PathXSystem)target;

            meshVisualizer = new(script);
            meshVisualizer.OnEnable();
        }

        protected void OnDisable()
        {
            meshVisualizer.OnDisable();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("Debug Mesh Filter", script.meshFilter)
                .OnValueChanged(value => script.meshFilter = value as MeshFilter)
                + PathXEngineInspector.Render(script.Engine)
                + RenderMeshVisualize(script.Engine)
            )
            .Render();

            serializedObject.ApplyModifiedProperties();
        }

        public SUIElement RenderMeshVisualize(PathXEngine engine)
        {
            if (engine == null) return SUIElement.Empty();
            return SEditorGUILayout.Group("Debug Mesh Visualize")
            .Content(
                SEditorGUILayout.Action(() =>
                {
                    foreach (var profile in engine.Profiles)
                    {
                        SEditorGUILayout.Vertical()
                        .Content(
                            SEditorGUILayout.Label(profile.domain.ToString())
                            .Bold()
                            .Align(TextAnchor.MiddleCenter)
                            + RenderNavSurface(profile.domain, profile.navSurface)
                        )
                        .Render();
                    }
                })
            );
        }

        private SUIElement RenderNavSurface(TriangleDomain domain, PathXNavMesh navSurface)
        {
            return SEditorGUILayout.Horizontal()
            .Content(
                SEditorGUILayout.Button("None")
                .OnClick(() => meshVisualizer.HideMesh(domain.ToString()))
                + SEditorGUILayout.Button("Solid")
                .OnClick(() =>
                {
                    meshVisualizer.ShowMesh(domain.ToString(), MeshGenerator.CreateMeshFromTriangles(navSurface.triangles));
                })
                + SEditorGUILayout.Button("Wire")
                .OnClick(() =>
                {
                    meshVisualizer.ShowMesh(domain.ToString(), MeshGenerator.CreateWireframeMeshFromTriangles(navSurface.triangles));
                })
            );
        }
    }
}
