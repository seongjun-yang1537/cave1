using Core;
using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class AgentModelStateInspector
    {
        private static bool foldBaseStat;

        public static SUIElement Render(AgentModelState state)
        {
            if (state == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Var("Life", state.life).OnValueChanged(value => state.life = value)
                    + SEditorGUILayout.Var("exp", state.exp).OnValueChanged(value => state.exp = value)
                )
                + SEditorGUILayout.Var("Aimtarget ID", state.aimtargetID).OnValueChanged(value => state.aimtargetID = value)
            );
        }

        public static SUIElement RenderGroup(AgentModelState state, bool fold, UnityAction<bool> setter)
        {
            if (state == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Agent Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(state)
                )
            );
        }
    }
}
