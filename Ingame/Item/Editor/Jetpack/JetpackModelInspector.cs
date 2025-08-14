using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class JetpackModelInspector
    {
        private static bool foldBaseStat = false;
        private static bool foldTotalStat = false;

        public static SUIElement Render(JetpackModel jetpackModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                JetpackModelDataInspector.Render(jetpackModel.Data)
                + SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Var("Fuel", jetpackModel.fuel)
                    .OnValueChanged(value => jetpackModel.SetFuel(value))
                    + JetpackStatInspector.RenderGroup(
                        jetpackModel.baseStat,
                        foldBaseStat,
                        value => foldBaseStat = value
                    )
                    + JetpackTotalStatInspector.RenderGroup(
                        jetpackModel.totalStat,
                        foldTotalStat,
                        value => foldTotalStat = value
                    )
                )
            );
        }

        public static SUIElement RenderGroup(JetpackModel jetpackModel, bool fold, UnityAction<bool> setter)
        {
            if (jetpackModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Jetpack Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(jetpackModel)
                )
            );
        }
    }
}