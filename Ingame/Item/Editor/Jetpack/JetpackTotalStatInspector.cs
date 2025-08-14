using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class JetpackTotalStatInspector
    {
        public static SUIElement Render(JetpackTotalStat stat)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Var("Fuel Max", stat.fuelMax)
                    + SEditorGUILayout.Var($"Fuel Ratio ({(stat.fuelRatio * 100f):F2}%)", stat.fuelRatio)
                )
                + SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Var("Jetpack Force", stat.jetpackForce)
                    + SEditorGUILayout.Var("Fuel Consumption Rate", stat.fuelConsumptionRate)
                    + SEditorGUILayout.Var("Fuel Recharge Rate", stat.fuelRechargeRate)
                    + SEditorGUILayout.Var("Fuel Recharge Delay", stat.fuelRechargeDelay)
                )
            );
        }

        public static SUIElement RenderGroup(JetpackTotalStat stat, bool fold, UnityAction<bool> setter)
        {
            if (stat == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Jetpack Total Stat", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(stat)
                )
            );
        }
    }
}