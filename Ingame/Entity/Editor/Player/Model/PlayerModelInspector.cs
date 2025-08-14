using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class PlayerModelInspector
    {
        private static bool foldBaseStat;
        private static bool foldTotalStat;
        private static bool foldJetpack;

        public static SUIElement Render(PlayerModel playerModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                PlayerModelDataInspector.Render(playerModel.Data)
                + SEditorGUILayout.Vertical("box")
                .Content(
                    PlayerStatInspector.RenderGroup(
                        playerModel.playerBaseStat,
                        foldBaseStat,
                        value => foldBaseStat = value
                    )
                    + PlayerTotalStatInspector.RenderGroup(
                        playerModel.playerTotalStat,
                        foldTotalStat,
                        value => foldTotalStat = value
                    )
                )
                + SEditorGUILayout.Vertical("box")
                .Content(
                    SEditorGUILayout.Var("Fuel", playerModel.jetpackModel.fuel)
                    .OnValueChanged(value => playerModel.jetpackModel.fuel = value)
                    + JetpackModelInspector.RenderGroup(
                        playerModel.jetpackModel,
                        foldJetpack,
                        value => foldJetpack = value
                    )
                )
                + SEditorGUILayout.Var("Gold", playerModel.gold)
                .OnValueChanged(value => playerModel.gold = value)
            );
        }

        public static SUIElement RenderGroup(PlayerModel playerModel, bool fold, UnityAction<bool> setter)
        {
            if (playerModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Player Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(playerModel)
                )
            );
        }
    }
}