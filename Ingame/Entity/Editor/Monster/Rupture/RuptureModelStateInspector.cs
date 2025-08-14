using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class RuptureModelStateInspector
    {
        public static SUIElement Render(RuptureModelState ruptureModelState)
        {
            if (ruptureModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("Explosion Range", ruptureModelState.explosionRange)
                    .OnValueChanged(value => ruptureModelState.explosionRange = value)
                + SEditorGUILayout.Var("Explosion Fuse Time", ruptureModelState.explosionFuseTime)
                    .OnValueChanged(value => ruptureModelState.explosionFuseTime = value)
            );
        }

        public static SUIElement RenderGroup(RuptureModelState ruptureModelState, bool fold, UnityAction<bool> setter)
        {
            if (ruptureModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Rupture Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(ruptureModelState)
                )
            );
        }
    }
}
