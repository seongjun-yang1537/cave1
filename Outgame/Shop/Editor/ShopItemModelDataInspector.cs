using Corelib.SUI;
using UnityEngine.Events;
using Ingame;
using Corelib.Utils;

namespace Outgame
{
    public static class ShopItemModelDataInspector
    {
        static bool foldItemModelData;

        public static SUIElement Render(ShopItemModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Object("Item Model Data", data.itemModelData, typeof(ItemModelData))
                    .OnValueChanged(value => data.itemModelData = value as ItemModelData)
                    + ItemModelDataInspector.RenderGroup(data.itemModelData, foldItemModelData, v => foldItemModelData = v)
                    + IntRangeeInspector.Render("Deliver Duration", data.deliverDurationRange)
                    + SEditorGUILayout.Var("Count", data.count)
                    .OnValueChanged(value => data.count = value)
                );
        }

        public static SUIElement RenderGroup(ShopItemModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("ShopItemModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}
