using Corelib.SUI;
using UnityEditor;

namespace Ingame
{
    [CustomEditor(typeof(ProjectileController))]
    public class EditorProjectileController : Editor
    {
        protected ProjectileController projectileController;

        protected void OnEnable()
        {
            projectileController = (ProjectileController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("Projectile View", projectileController.projectileView)
                + SEditorGUILayout.Var("Current Life Time", projectileController.currentLifeTime)
            )
            .Render();
        }
    }
}
