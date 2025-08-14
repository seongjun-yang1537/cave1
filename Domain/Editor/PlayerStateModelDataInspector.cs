using Corelib.SUI;
using UnityEngine.Events;
using Ingame;

namespace Domain
{
    public static class GamePlayerModelDataInspector
    {
        static bool foldPlayerModel;

        public static SUIElement Render(GamePlayerModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    PlayerModelDataInspector.RenderGroup(data.playerModel, foldPlayerModel, v => foldPlayerModel = v)
                    + SEditorGUILayout.Var("activeQuests", data.activeQuests).OnValueChanged(v => data.activeQuests = v)
                );
        }

        public static SUIElement RenderGroup(GamePlayerModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("GamePlayerModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}
