using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class ProjectileModelStateInspector
    {
        public static SUIElement Render(ProjectileModelState projectileModelData)
        {
            if (projectileModelData == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("Damage", projectileModelData.damage)
                    .OnValueChanged(value => projectileModelData.damage = value)
                + SEditorGUILayout.Var("Speed", projectileModelData.speed)
                    .OnValueChanged(value => projectileModelData.speed = value)
                + SEditorGUILayout.Var("Life Time", projectileModelData.lifeTime)
                    .OnValueChanged(value => projectileModelData.lifeTime = value)
                + SEditorGUILayout.Var("Range", projectileModelData.range)
                    .OnValueChanged(value => projectileModelData.range = value)
                + SEditorGUILayout.Var("Target Layer", projectileModelData.targetLayer)
                    .OnValueChanged(value => projectileModelData.targetLayer = value)
            );
        }

        public static SUIElement RenderGroup(ProjectileModelState projectileModelData, bool fold, UnityAction<bool> setter)
        {
            if (projectileModelData == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Projectile Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(projectileModelData)
                )
            );
        }
    }
}
