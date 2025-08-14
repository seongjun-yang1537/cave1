using Core;
using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class PlayerModelDataInspector
    {
        static bool foldPlayerStat;
        static bool foldJetpack;

        public static SUIElement Render(PlayerModelData data)
        {
            if (data == null) return SUIElement.Empty();
            if (data.jetpackModelData == null)
                data.jetpackModelData = new();

            return SEditorGUILayout.Vertical()
            .Content(
                PlayerStatInspector.RenderGroup(data.playerBaseStat, foldPlayerStat, v => foldPlayerStat = v)
                + SEditorGUILayout.Var("inputConfig", data.inputConfig).OnValueChanged(v => data.inputConfig = v)

                + SEditorGUILayout.Vertical("box")
                .Content(
                    JetpackModelDataInspector.RenderGroup(data.jetpackModelData, foldJetpack, v => foldJetpack = v)
                )
            );
        }

        public static SUIElement RenderGroup(PlayerModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("PlayerModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}
