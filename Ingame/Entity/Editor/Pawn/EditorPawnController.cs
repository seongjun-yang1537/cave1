using Corelib.SUI;
using UnityEditor;

namespace Ingame
{
    [CustomEditor(typeof(PawnController))]
    public class EditorPawnController : EditorAgentController
    {
        protected PawnController pawnController;

        protected void OnEnable()
        {
            base.OnEnable();
            pawnController = (PawnController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("Pawn View", pawnController.pawnView)
                + SEditorGUILayout.Var("Hand Controller", pawnController.handController)
            )
            .Render();
        }
    }
}
