using Corelib.SUI;
using Corelib.Utils;
using Ingame;
using PathX;
using UnityEditor;

namespace World
{
    [CustomEditor(typeof(GameWorld))]
    public class EditorGameWorld : Editor
    {
        GameWorld script;

        WorldSystem worldSystem;
        PathXSystem pathXSystem;
        EntitySystem entitySystem;
        protected void OnEnable()
        {
            script = (GameWorld)target;

            worldSystem = script.transform.GetComponentInDirectChildren<WorldSystem>();
            pathXSystem = script.transform.GetComponentInDirectChildren<PathXSystem>();
            entitySystem = script.transform.GetComponentInDirectChildren<EntitySystem>();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Button("Clear World Data Instance")
                .OnClick(() => script.worldData = null)
                + RenderWorldSystem()
                + RenderEntitySystem()
                + RenderPathXSystem()
            )
            .Render();
        }

        private SUIElement RenderWorldSystem()
        {
            return SEditorGUILayout.Vertical("box")
            .Content(
                SEditorGUILayout.Horizontal()
                .Content(
                    SEditorGUILayout.Var("WorldSystem", worldSystem)
                    + SEditorGUILayout.Button("Create")
                    .Width(60)
                    .Where(() => worldSystem == null)
                )
                + SEditorGUILayout.HelpBox($"Missing WorldSystem", MessageType.Error)
                .Where(() => worldSystem == null)
            );
        }

        private SUIElement RenderEntitySystem()
        {
            return SEditorGUILayout.Vertical("box")
            .Content(
                SEditorGUILayout.Horizontal()
                .Content(
                    SEditorGUILayout.Var("EntitySystem", entitySystem)
                    + SEditorGUILayout.Button("Create")
                    .Width(60)
                    .Where(() => entitySystem == null)
                )
                + SEditorGUILayout.HelpBox($"Missing EntitySystem", MessageType.Error)
                .Where(() => entitySystem == null)
            );
        }

        private SUIElement RenderPathXSystem()
        {
            return SEditorGUILayout.Vertical("box")
            .Content(
                SEditorGUILayout.Horizontal()
                .Content(
                    SEditorGUILayout.Var("PathXSystem", pathXSystem)
                    + SEditorGUILayout.Button("Create")
                    .Width(60)
                    .Where(() => pathXSystem == null)
                )
                + SEditorGUILayout.HelpBox($"Missing PathXSystem", MessageType.Error)
                .Where(() => pathXSystem == null)
            );
        }
    }
}