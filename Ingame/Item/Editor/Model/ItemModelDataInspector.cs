using Corelib.SUI;
using UnityEngine.Events;
using Corelib.Utils;
using Core;

namespace Ingame
{
    public static class ItemModelDataInspector
    {
        static bool foldEquipmentStat;

        public static SUIElement Render(ItemModelData data)
        {
            if (data == null) return SUIElement.Empty();
            if (data.baseEquipmentStat == null) data.baseEquipmentStat = new();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Var("Name", data.name).OnValueChanged(v => data.name = v)
                    + SEditorGUILayout.Var("Description", data.description).OnValueChanged(v => data.description = v)
                    + SEditorGUILayout.Var("Item ID", data.itemID).OnValueChanged(v => data.itemID = v)
                    + SEditorGUILayout.Var("Equipment Type", data.equipmentType).OnValueChanged(v => data.equipmentType = v)
                    + SEditorGUILayout.Var("Max Stackable", data.maxStackable).OnValueChanged(v => data.maxStackable = v)
                    + SEditorGUILayout.Var("Is Acquireable", data.isAcquireable).OnValueChanged(v => data.isAcquireable = v)
                    + EquipmentStatInspector.RenderGroup(data.baseEquipmentStat, foldEquipmentStat, v => foldEquipmentStat = v)
                );
        }

        public static SUIElement RenderGroup(ItemModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("ItemModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}

