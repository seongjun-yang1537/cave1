using Corelib.SUI;
using UnityEngine.Events;

namespace Domain
{
    public static class GamePlayerModelStateInspector
    {
        public static SUIElement Render(GamePlayerModelState state)
        {
            if (state == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
            .Content(
            );
        }

        public static SUIElement RenderGroup(GamePlayerModelState state, bool fold, UnityAction<bool> setter)
        {
            if (state == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("GamePlayerModelState", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(state)
                    )
                );
        }
    }
}
