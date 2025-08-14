using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class InventorySlotModelStateInspector
    {
        static bool foldItemModelState;
        public static SUIElement Render(InventorySlotModelState state)
        {
            if (state == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("Owner Container", state.ownerContainer)
                .OnValueChanged(value => state.ownerContainer = value)
                + SEditorGUILayout.Var("Slot ID", state.slotID)
                .OnValueChanged(value => state.slotID = value)
                + ItemModelStateInspector.RenderGroup(state.itemModelState, foldItemModelState, v => foldItemModelState = v)
            );
        }

        public static SUIElement RenderGroup(InventorySlotModelState state, bool fold, UnityAction<bool> setter)
        {
            if (state == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Inventory Slot Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(state)
                )
            );
        }
    }
}
