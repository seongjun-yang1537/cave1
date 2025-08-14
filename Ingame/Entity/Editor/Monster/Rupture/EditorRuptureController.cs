using Corelib.SUI;
using UnityEditor;

namespace Ingame
{
    [CustomEditor(typeof(RuptureController))]
    public class EditorRuptureController : EditorMonsterController
    {
        protected RuptureController ruptureController;

        protected void OnEnable()
        {
            base.OnEnable();
            ruptureController = (RuptureController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Label("Current State: " + ruptureController.CurrentState.ToString())
            )
            .Render();
        }
    }
}
