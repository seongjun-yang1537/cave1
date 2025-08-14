using Corelib.SUI;
using UnityEngine.Events;

namespace Domain
{
    public static class GameModelStateInspector
    {
        static bool foldGamePlayer;
        static bool foldCalendar;

        public static SUIElement Render(GameModelState state)
        {
            if (state == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    GamePlayerModelStateInspector.RenderGroup(state.gamePlayer, foldGamePlayer, v => foldGamePlayer = v)
                    + SEditorGUILayout.Var("calendar", state.calendar).OnValueChanged(v => state.calendar = v)
                    + SEditorGUILayout.Var("availableQuests", state.availableQuests).OnValueChanged(v => state.availableQuests = v)
                );
        }

        public static SUIElement RenderGroup(GameModelState state, bool fold, UnityAction<bool> setter)
        {
            if (state == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("GameModelState", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(state)
                    )
                );
        }
    }
}
