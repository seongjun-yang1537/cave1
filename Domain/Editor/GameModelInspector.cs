using Corelib.SUI;
using UnityEngine.Events;

namespace Domain
{
    public static class GameModelInspector
    {
        static bool foldGamePlayer;
        static bool foldCalendar;

        public static SUIElement Render(GameModel model)
        {
            if (model == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    GamePlayerModelInspector.RenderGroup(model.gamePlayerModel, foldGamePlayer, v => foldGamePlayer = v)
                    + SEditorGUILayout.FoldGroup("calendarModel", foldCalendar)
                        .OnValueChanged(v => foldCalendar = v)
                        .Content(
                            model.calendarModel == null ? SUIElement.Empty() : SEditorGUILayout.Var("currentDay", model.calendarModel.currentDay)
                        )
                    + SEditorGUILayout.Var("availableQuests", model.availableQuests).OnValueChanged(v => model.availableQuests = v)
                );
        }

        public static SUIElement RenderGroup(GameModel model, bool fold, UnityAction<bool> setter)
        {
            if (model == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("GameModel", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(model)
                    )
                );
        }
    }
}
