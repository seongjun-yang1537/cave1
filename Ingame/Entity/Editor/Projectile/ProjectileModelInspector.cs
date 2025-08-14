using Corelib.SUI;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    public static class ProjectileModelInspector
    {
        public static SUIElement Render(ProjectileModel projectileModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                ProjectileModelDataInspector.Render(projectileModel.Data)
                + SEditorGUILayout.Var("Owner ID", projectileModel.owner.entityID)

                + SEditorGUILayout.Var("Follow Target", projectileModel.followTarget)
            );
        }

        public static SUIElement RenderGroup(ProjectileModel projectileModel, bool fold, UnityAction<bool> setter)
        {
            if (projectileModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Projectile Model", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(projectileModel)
                    )
            );
        }
    }
}