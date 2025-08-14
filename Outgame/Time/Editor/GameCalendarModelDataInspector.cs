using Corelib.SUI;
using UnityEngine.Events;
using GameTime;
using Core;

namespace Outgame
{
    public static class GameCalendarModelDataInspector
    {
        public static SUIElement Render(GameCalendarModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Var("currentDay", data.currentDay).OnValueChanged(v => data.currentDay = v)
                );
        }

        public static SUIElement RenderGroup(GameCalendarModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("GameCalendarModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}
