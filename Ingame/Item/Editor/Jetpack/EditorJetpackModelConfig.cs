using Corelib.SUI;
using UnityEditor;

namespace Ingame
{
    [CustomEditor(typeof(JetpackModelConfig))]
    public class EditorJetpackModelConfig : Editor
    {
        JetpackModelConfig script;
        protected void OnEnable()
        {
            script = (JetpackModelConfig)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SEditorGUI.ChangeCheck(
                target,
                SEditorGUILayout.Vertical()
                .Content(
                    JetpackModelInspector.Render(script.template)
                )
            )
            .Render();
        }
    }
}