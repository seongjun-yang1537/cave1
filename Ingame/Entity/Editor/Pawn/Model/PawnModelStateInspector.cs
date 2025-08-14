using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class PawnModelStateInspector
    {
        public static SUIElement Render(PawnModelState state)
        {
            if (state == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("Now Speed", state.nowSpeed)
                .OnValueChanged(value => state.nowSpeed = value)
            );
        }

        public static SUIElement RenderGroup(PawnModelState state, bool fold, UnityAction<bool> setter)
        {
            if (state == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Pawn Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(state)
                )
            );
        }
    }
}
