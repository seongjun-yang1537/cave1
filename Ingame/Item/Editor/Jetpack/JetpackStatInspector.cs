using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class JetpackStatInspector
    {
        public static SUIElement Render(JetpackStat stat)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Var("Fuel Max", stat.fuelMax)
                    .OnValueChanged(value => stat.fuelMax = value)
                )
                + SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Var("Jetpack Force", stat.jetpackForce)
                    .OnValueChanged(value => stat.jetpackForce = value)
                    + SEditorGUILayout.Var("Fuel Consumption Rate", stat.fuelConsumptionRate)
                    .OnValueChanged(value => stat.fuelConsumptionRate = value)
                    + SEditorGUILayout.Var("Fuel Recharge Rate", stat.fuelRechargeRate)
                    .OnValueChanged(value => stat.fuelRechargeRate = value)
                    + SEditorGUILayout.Var("Fuel Recharge Delay", stat.fuelRechargeDelay)
                    .OnValueChanged(value => stat.fuelRechargeDelay = value)
                )
            );
        }

        public static SUIElement RenderGroup(JetpackStat stat, bool fold, UnityAction<bool> setter)
        {
            if (stat == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Jetpack Stat", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(stat)
                )
            );
        }
    }
}