using Corelib.SUI;
using UnityEngine.Events;
using Ingame;
using Quest;

namespace Domain
{
    public static class GamePlayerModelInspector
    {
        static bool foldPlayerModel;

        public static SUIElement Render(GamePlayerModel model)
        {
            if (model == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    PlayerModelInspector.RenderGroup(model.playerModel, foldPlayerModel, v => foldPlayerModel = v)
                    + SEditorGUILayout.Var("activeQuests", model.activeQuests).OnValueChanged(v => model.activeQuests = v)
                );
        }

        public static SUIElement RenderGroup(GamePlayerModel model, bool fold, UnityAction<bool> setter)
        {
            if (model == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("GamePlayerModel", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(model)
                    )
                );
        }
    }
}
