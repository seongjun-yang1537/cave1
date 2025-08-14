using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class InventorySlotModelInspector
    {
        static bool foldData;
        static bool foldItemModel;
        public static SUIElement Render(InventorySlotModel model)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                // InventorySlotModelDataInspector.RenderGroup(model.data, foldData, v => foldData = v)
                SEditorGUILayout.Var("Owner Container", model.ownerContainer)
                .OnValueChanged(value => model.ownerContainer = value)
                + SEditorGUILayout.Var("Slot ID", model.slotID)
                .OnValueChanged(value => model.slotID = value)
                + ItemModelInspector.RenderGroup(model.itemModel, foldItemModel, v => foldItemModel = v)
            );
        }

        public static SUIElement RenderGroup(InventorySlotModel model, bool fold, UnityAction<bool> setter)
        {
            if (model == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Inventory Slot Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(model)
                )
            );
        }
    }
}
