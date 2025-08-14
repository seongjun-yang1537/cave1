using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class AgentTotalStatInspector
    {
        public static SUIElement Render(AgentTotalStat stat)
        {
            return SEditorGUILayout.Vertical("box")
            .Content(
                SEditorGUILayout.Var("Life Max", stat.lifeMax)
                + SEditorGUILayout.Var($"Life Ratio ({(stat.lifeRatio * 100f):F2}%)", stat.lifeRatio)
                + SEditorGUILayout.Var("Life Regen", stat.lifeRegen)
                + SEditorGUILayout.Var("Attack", stat.attack)
                + SEditorGUILayout.Var("Defense", stat.defense)
                + SEditorGUILayout.Var("Attack Speed", stat.attackSpeed)
                + SEditorGUILayout.Var("Critical Chance", stat.criticalChance)
                + SEditorGUILayout.Var("Critical Multiplier", stat.criticalMultiplier)
            );
        }

        public static SUIElement RenderGroup(AgentTotalStat stat, bool fold, UnityAction<bool> setter)
        {
            if (stat == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Agent Total Stat", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(stat)
                )
            );
        }
    }
}