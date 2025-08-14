using System;
using System.Collections.Generic;
using Corelib.SUI;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    public static class InventoryModelInspector
    {
        static int selectedBagIdx;
        static ItemID bagItemID;
        static int bagItemCount;
        static int selectedQuickIdx;
        static ItemID quickItemID;
        static int quickItemCount;
        static int selectedEquipIdx;
        static ItemID equipItemID;
        static int equipItemCount;

        public static SUIElement Render(InventoryModel inventory)
        {
            if (inventory == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
            .Content(
                RenderBag(inventory)
                + RenderQuickSlot(inventory)
                + RenderEquipment(inventory)
            );
        }

        public static SUIElement RenderGroup(InventoryModel inventory, bool fold, UnityAction<bool> setter)
        {
            if (inventory == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Inventory", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(inventory)
                )
            );
        }

        static SUIElement RenderBag(InventoryModel inventory)
        {
            List<GUIContent> guiContents = new();
            foreach (BagSlotID slotID in Enum.GetValues(typeof(BagSlotID)))
            {
                InventorySlotModel slot = inventory.GetItemSlot(slotID);
                guiContents.Add(CreateItemModelGUIContent(slot.itemModel));
            }

            return SEditorGUILayout.Group("Bag")
            .Content(
                SGUILayout.SelectionGrid(selectedBagIdx, guiContents, 5)
                .OnValueChanged(value => selectedBagIdx = value)
                + SEditorGUILayout.Horizontal()
                .LabelWidth(60)
                .Content(
                    SEditorGUILayout.Var("Item ID", bagItemID)
                    .OnValueChanged(value => bagItemID = (ItemID)value)
                    + SEditorGUILayout.Var("Count", bagItemCount)
                    .OnValueChanged(value => bagItemCount = value)
                )
                + SEditorGUILayout.Horizontal()
                .Content(
                    SEditorGUILayout.Button("Apply")
                    .OnClick(() =>
                    {
                        if (bagItemCount > 0)
                            inventory.SetItemSlot(
                                (BagSlotID)selectedBagIdx,
                                ItemModelFactory.Create(new ItemModelState { itemID = bagItemID, count = bagItemCount })
                            );
                    })
                    + SEditorGUILayout.Button("Remove")
                    .OnClick(() =>
                    {
                        inventory.SetItemSlot((BagSlotID)selectedBagIdx, ItemModel.Empty);
                    })
                )
            );
        }

        static SUIElement RenderQuickSlot(InventoryModel inventory)
        {
            List<GUIContent> guiContents = new();
            foreach (QuickSlotID slotID in Enum.GetValues(typeof(QuickSlotID)))
            {
                InventorySlotModel slot = inventory.GetItemSlot(slotID);
                guiContents.Add(CreateItemModelGUIContent(slot.itemModel));
            }

            return SEditorGUILayout.Group("Quick Slot")
            .Content(
                SGUILayout.SelectionGrid(selectedQuickIdx, guiContents, 7)
                .OnValueChanged(value => selectedQuickIdx = value)
                + SEditorGUILayout.Horizontal()
                .LabelWidth(60)
                .Content(
                    SEditorGUILayout.Var("Item ID", quickItemID)
                    .OnValueChanged(value => quickItemID = (ItemID)value)
                    + SEditorGUILayout.Var("Count", quickItemCount)
                    .OnValueChanged(value => quickItemCount = value)
                )
                + SEditorGUILayout.Horizontal()
                .Content(
                    SEditorGUILayout.Button("Apply")
                    .OnClick(() =>
                    {
                        if (quickItemCount > 0)
                            inventory.SetItemSlot(
                                (QuickSlotID)selectedQuickIdx,
                                ItemModelFactory.Create(new ItemModelState { itemID = quickItemID, count = quickItemCount })
                            );
                    })
                    + SEditorGUILayout.Button("Remove")
                    .OnClick(() =>
                    {
                        inventory.SetItemSlot((QuickSlotID)selectedQuickIdx, ItemModel.Empty);
                    })
                )
            );
        }

        static SUIElement RenderEquipment(InventoryModel inventory)
        {
            List<GUIContent> guiContents = new();
            foreach (EquipmentSlotID slotID in Enum.GetValues(typeof(EquipmentSlotID)))
            {
                InventorySlotModel slot = inventory.GetItemSlot(slotID);
                guiContents.Add(CreateItemModelGUIContent(slot.itemModel));
            }

            return SEditorGUILayout.Group("Equipment")
            .Content(
                SGUILayout.SelectionGrid(selectedEquipIdx, guiContents, 7)
                .OnValueChanged(value => selectedEquipIdx = value)
                + SEditorGUILayout.Horizontal()
                .LabelWidth(60)
                .Content(
                    SEditorGUILayout.Var("Item ID", equipItemID)
                    .OnValueChanged(value => equipItemID = (ItemID)value)
                    + SEditorGUILayout.Var("Count", equipItemCount)
                    .OnValueChanged(value => equipItemCount = value)
                )
                + SEditorGUILayout.Horizontal()
                .Content(
                    SEditorGUILayout.Button("Apply")
                    .OnClick(() =>
                    {
                        if (equipItemCount > 0)
                            inventory.SetItemSlot(
                                (EquipmentSlotID)selectedEquipIdx,
                                ItemModelFactory.Create(new ItemModelState { itemID = equipItemID, count = equipItemCount })
                            );
                    })
                    + SEditorGUILayout.Button("Remove")
                    .OnClick(() =>
                    {
                        inventory.SetItemSlot((EquipmentSlotID)selectedEquipIdx, ItemModel.Empty);
                    })
                )
            );
        }

        static GUIContent CreateItemModelGUIContent(ItemModel itemModel)
        {
            if (itemModel == null || itemModel.IsEmpty)
                return new GUIContent();

            Texture2D icon = ItemDB.GetEditorIconTexture(itemModel.itemID);
            string label = itemModel.count > 1 ? $"x{itemModel.count}" : "";
            string tooltip = "";

            return new GUIContent(icon, $"{tooltip} {label}");
        }
    }
}
