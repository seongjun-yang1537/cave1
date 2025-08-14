using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class PlayerModelStateInspector
    {
        private static bool foldJetpack;
        public static SUIElement Render(PlayerModelState state)
        {
            if (state == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Var("Jetpack Model State", state.jetpackModelState)
                    .OnValueChanged(v => state.jetpackModelState = v)
                    + JetpackModelStateInspector.RenderGroup(state.jetpackModelState,
                        foldJetpack,
                        value => foldJetpack = value
                    )
                )
                + SEditorGUILayout.Var("Gold", state.gold)
                .OnValueChanged(value => state.gold = value)
            );
        }

        public static SUIElement RenderGroup(PlayerModelState state, bool fold, UnityAction<bool> setter)
        {
            if (state == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Player Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(state)
                )
            );
        }
    }
}
