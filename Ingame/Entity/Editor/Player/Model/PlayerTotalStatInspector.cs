using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class PlayerTotalStatInspector
    {
        public static SUIElement Render(PlayerTotalStat stat)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Var("Oxygen Max", stat.oxygenMax)
                    + SEditorGUILayout.Var($"Oxygen Ratio ({(stat.oxygenRatio * 100f):F2}%)", stat.oxygenRatio)
                )
                + SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Var("Stamina Max", stat.steminaMax)
                    + SEditorGUILayout.Var($"Stamina Ratio ({(stat.steminaRatio * 100f):F2}%)", stat.steminaRatio)
                )
            );
        }

        public static SUIElement RenderGroup(PlayerTotalStat stat, bool fold, UnityAction<bool> setter)
        {
            if (stat == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Player Total Stat", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(stat)
                )
            );
        }
    }
}