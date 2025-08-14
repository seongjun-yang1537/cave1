using System;
using Corelib.SUI;
using UnityEditor;

namespace Ingame
{
    [CustomEditor(typeof(JetpackModelState))]
    [Serializable]
    public class EditorJetpackModelData : Editor
    {
        JetpackModelState script;
        protected void OnEnable()
        {
            // script = (JetpackModelData)target;
        }

        public override void OnInspectorGUI()
        {
            // SEditorGUI.ChangeCheck(
            //     script,
            //     SEditorGUILayout.Vertical()
            //     .Content(
            //         JetpackModelStateInspector.Render(script)
            //     )
            // ).Render();
        }
    }
}
