using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class ItemModelStateInspector
    {
        public static SUIElement Render(ItemModelState state)
        {
            if (state == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("Item ID", state.itemID)
                .OnValueChanged(value => state.itemID = (ItemID)value)
                + SEditorGUILayout.Var("Count", state.count)
                .OnValueChanged(value => state.count = value)
            );
        }

        public static SUIElement RenderGroup(ItemModelState state, bool fold, UnityAction<bool> setter)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Item Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(state)
                )
            );
        }
    }
}
