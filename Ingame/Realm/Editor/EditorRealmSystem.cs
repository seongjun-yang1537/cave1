using Corelib.SUI;
using UnityEditor;

namespace Realm
{
    [CustomEditor(typeof(RealmSystem))]
    public class EditorRealmSystem : Editor
    {
        RealmSystem script;
        protected virtual void OnEnable()
        {
            script = (RealmSystem)target;
        }

        bool foldModel = true;
        float inputDepth = 0;


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SEditorGUI.ChangeCheck(
                target,
                SEditorGUILayout.Vertical("box")
                .Content(
                    RealmModelInspector.RenderGroup(script.model, foldModel, value => foldModel = value)
                    + SEditorGUILayout.Button("Descend World")
                    .OnClick(async () => await RealmSystem.DescendWorld())
                    + SEditorGUILayout.Horizontal()
                    .Content(
                        SEditorGUILayout.Var("", inputDepth)
                        .OnValueChanged(value => inputDepth = value)
                        + SEditorGUILayout.Button("Rebuld World By Depth")
                        .OnClick(async () => await RealmSystem.Rebuild(inputDepth))
                    )
                )
            )
            .Render();
        }
    }
}