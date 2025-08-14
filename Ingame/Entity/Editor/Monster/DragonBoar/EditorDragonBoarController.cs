using Corelib.SUI;
using UnityEditor;

namespace Ingame
{
    [CustomEditor(typeof(DragonBoarController))]
    public class EditorDragonBoarController : EditorMonsterController
    {
        protected DragonBoarController dragonBoarController;

        protected void OnEnable()
        {
            base.OnEnable();
            dragonBoarController = (DragonBoarController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Label("Current State: " + dragonBoarController.CurrentState.ToString())
            )
            .Render();
        }
    }
}
