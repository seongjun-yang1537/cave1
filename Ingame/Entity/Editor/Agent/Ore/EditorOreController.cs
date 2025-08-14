using Corelib.SUI;
using UnityEditor;

namespace Ingame
{
    [CustomEditor(typeof(OreController))]
    public class EditorOreController : EditorAgentController
    {
        protected OreController oreController;

        protected void OnEnable()
        {
            base.OnEnable();
            oreController = (OreController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("Ore View", oreController.oreView)
            )
            .Render();
        }
    }
}
