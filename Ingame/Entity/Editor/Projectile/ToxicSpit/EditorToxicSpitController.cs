using Corelib.SUI;
using UnityEditor;

namespace Ingame
{
    [CustomEditor(typeof(ToxicSpitController))]
    public class EditorToxicSpitController : EditorProjectileController
    {
        protected ToxicSpitController toxicSpitController;

        protected void OnEnable()
        {
            base.OnEnable();
            toxicSpitController = (ToxicSpitController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("ToxicSpit View", toxicSpitController.toxicSpitView)
            )
            .Render();
        }
    }
}
