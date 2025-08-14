using Corelib.SUI;
using UnityEngine.Events;
using Outgame;

namespace Domain
{
    public static class GameModelDataInspector
    {
        static bool foldPlayerState;
        static bool foldCalendar;

        public static SUIElement Render(GameModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    GamePlayerModelDataInspector.RenderGroup(data.gamePlayer, foldPlayerState, v => foldPlayerState = v)
                    + GameCalendarModelDataInspector.RenderGroup(data.calendar, foldCalendar, v => foldCalendar = v)
                );
        }

        public static SUIElement RenderGroup(GameModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("GameModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}
