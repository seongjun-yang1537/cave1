using Corelib.SUI;
using UnityEditor;

namespace Ingame
{
    [CustomEditor(typeof(CreeplingerController))]
    public class EditorCreeplingerController : EditorMonsterController
    {
        protected CreeplingerController creeplingerController;

        protected void OnEnable()
        {
            base.OnEnable();
            creeplingerController = (CreeplingerController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("Creeplinger View", creeplingerController.creeplingerView)
                + SEditorGUILayout.Label("Current State: " + creeplingerController.CurrentState.ToString())
            )
            .Render();
        }
    }
}
