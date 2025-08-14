using Corelib.SUI;
using UnityEditor;
using UnityEngine;

namespace Ingame
{
    [CustomEditor(typeof(DebugItemSpawner))]
    public class EditorDebugItemSpawner : Editor
    {
        DebugItemSpawner script;
        protected void OnEnable()
        {
            script = (DebugItemSpawner)target;
        }

        public override void OnInspectorGUI()
        {
            SEditorGUILayout.Vertical("box")
            .Content(
                SEditorGUILayout.Label("[Debug]")
                .Bold()
                .Align(TextAnchor.MiddleCenter)
                + SEditorGUILayout.Action(() => base.OnInspectorGUI())
                + SEditorGUILayout.Button("Spawn")
                .OnClick(() => script.Spawn())
            )
            .Render();
        }
    }
}