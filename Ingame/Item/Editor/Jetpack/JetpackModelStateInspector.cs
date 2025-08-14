using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class JetpackModelStateInspector
    {
        public static SUIElement Render(JetpackModelState jetpackModelState)
        {
            if (jetpackModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Var("Fuel", jetpackModelState.fuel)
                    .OnValueChanged(value => jetpackModelState.fuel = value)
                    + SEditorGUILayout.Var("State", jetpackModelState.state)
                    .OnValueChanged(value => jetpackModelState.state = value)
                )
            );
        }

        public static SUIElement RenderGroup(JetpackModelState jetpackModelState, bool fold, UnityAction<bool> setter)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Jetpack Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(jetpackModelState)
                )
            );
        }
    }
}