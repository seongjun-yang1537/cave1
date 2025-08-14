using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class PawnModelInspector
    {
        private static bool foldPhysics;
        private static bool foldBaseStat;
        private static bool foldTotalStat;

        public static SUIElement Render(PawnModel pawnModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                PawnModelDataInspector.Render(pawnModel.Data)
                + PawnPhysicsSettingInspector.RenderGroup(
                    pawnModel.physicsSetting,
                    foldPhysics,
                    value => foldPhysics = value
                )
                + SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Var("Now Speed", pawnModel.nowSpeed)
                    .OnValueChanged(value => pawnModel.nowSpeed = value)
                    + PawnStatInspector.RenderGroup(
                        pawnModel.pawnBaseStat,
                        foldBaseStat, value =>
                        foldBaseStat = value
                    )
                    + PawnTotalStatInspector.RenderGroup(
                        pawnModel.pawnTotalStat,
                        foldTotalStat,
                        value => foldTotalStat = value
                    )
                )
            );
        }

        public static SUIElement RenderGroup(PawnModel pawnModel, bool fold, UnityAction<bool> setter)
        {
            if (pawnModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Pawn Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(pawnModel)
                )
            );
        }
    }
}