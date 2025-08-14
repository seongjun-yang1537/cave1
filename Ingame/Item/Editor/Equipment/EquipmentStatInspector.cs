using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class EquipmentStatInspector
    {
        public static SUIElement Render(EquipmentStat stat)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Var("Life Max", stat.lifeMax)
                    .OnValueChanged(value => stat.lifeMax = value)
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
                    SEditorGUILayout.Var("Move Speed", stat.moveSpeed)
                    .OnValueChanged(value => stat.moveSpeed = value)
                )
            );
        }

        public static SUIElement RenderGroup(EquipmentStat stat, bool fold, UnityAction<bool> setter)
        {
            if (stat == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Equipment Stat", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(stat)
                )
            );
        }
    }
}
