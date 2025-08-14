using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class AgentStatInspector
    {
        public static SUIElement Render(AgentStat stat)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Var("Level", stat.level)
                    .OnValueChanged(value => stat.level = value)
                    + SEditorGUILayout.Var("Exp Max", stat.expMax)
                    .OnValueChanged(value => stat.expMax = value)
                )
                + SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Var("Life Max", stat.lifeMax)
                    .OnValueChanged(value => stat.lifeMax = value)
                    + SEditorGUILayout.Var("Life Regen", stat.lifeRegen)
                    .OnValueChanged(value => stat.lifeRegen = value)
                )
                + SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Var("Attack", stat.attack)
                    .OnValueChanged(value => stat.attack = value)
                    + SEditorGUILayout.Var("Defense", stat.defense)
                    .OnValueChanged(value => stat.defense = value)
                    + SEditorGUILayout.Var("Attack Speed", stat.attackSpeed)
                    .OnValueChanged(value => stat.attackSpeed = value)
                    + SEditorGUILayout.Var("Critical Chance", stat.criticalChance)
                    .OnValueChanged(value => stat.criticalChance = value)
                    + SEditorGUILayout.Var("Critical Multiplier", stat.criticalMultiplier)
                    .OnValueChanged(value => stat.criticalMultiplier = value)
                )
                + SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Var("Stamina Max", stat.staminaMax)
                    .OnValueChanged(value => stat.staminaMax = value)
                    + SEditorGUILayout.Var("Stamina Regen", stat.staminaRegen)
                    .OnValueChanged(value => stat.staminaRegen = value)
                )
            );
        }

        public static SUIElement RenderGroup(AgentStat stat, bool fold, UnityAction<bool> setter)
        {
            if (stat == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Agent Stat", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(stat)
                )
            );
        }
    }
}
