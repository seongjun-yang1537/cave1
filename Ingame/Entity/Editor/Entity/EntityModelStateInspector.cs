using Corelib.SUI;
using PathX;
using UnityEngine.Events;

namespace Ingame
{
    public static class EntityModelStateInspector
    {
        public static SUIElement Render(EntityModelState state)
        {
            if (state == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
            );
        }

        public static SUIElement RenderGroup(EntityModelState state, bool fold, UnityAction<bool> setter)
        {
            if (state == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Entity Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(state)
                )
            );
        }
    }
}
