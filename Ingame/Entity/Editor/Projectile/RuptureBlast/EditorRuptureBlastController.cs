using Corelib.SUI;
using UnityEditor;

namespace Ingame
{
    [CustomEditor(typeof(RuptureBlastController))]
    public class EditorRuptureBlastController : EditorProjectileController
    {
        protected RuptureBlastController ruptureBlastController;

        protected void OnEnable()
        {
            base.OnEnable();
            ruptureBlastController = (RuptureBlastController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("RuptureBlast View", ruptureBlastController.ruptureBlastView)
            )
            .Render();
        }
    }
}
