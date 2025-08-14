using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class PlayerStatInspector
    {
        public static SUIElement Render(PlayerStat stat)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Var("Oxygen Max", stat.oxygenMax)
                    .OnValueChanged(value => stat.oxygenMax = value)
                )
                + SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Var("Stamina Max", stat.steminaMax)
                    .OnValueChanged(value => stat.steminaMax = value)
                )
            );
        }

        public static SUIElement RenderGroup(PlayerStat stat, bool fold, UnityAction<bool> setter)
        {
            if (stat == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Player Stat", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(stat)
                )
            );
        }
    }
}