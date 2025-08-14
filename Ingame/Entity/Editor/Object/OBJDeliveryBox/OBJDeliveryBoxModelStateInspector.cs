using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class OBJDeliveryBoxModelStateInspector
    {
        static bool foldItemModelState;
        public static SUIElement Render(OBJDeliveryBoxModelState state)
        {
            if (state == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
            .Content(
                ItemModelStateInspector.RenderGroup(state.itemModelState, foldItemModelState, v => foldItemModelState = v)
            );
        }

        public static SUIElement RenderGroup(OBJDeliveryBoxModelState state, bool fold, UnityAction<bool> setter)
        {
            if (state == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("OBJDeliveryBox State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(state)
                )
            );
        }
    }
}