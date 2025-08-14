using Corelib.SUI;
using UnityEngine.Events;
using Ingame;

namespace Outgame
{
    public static class ShopItemModelInspector
    {
        static bool foldItemModel;

        public static SUIElement Render(ShopItemModel model)
        {
            if (model == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    ItemModelInspector.RenderGroup(model.itemModel, foldItemModel, v => foldItemModel = v)
                    + SEditorGUILayout.Var("Phase", model.phase).OnValueChanged(v => model.phase = (ShopItemPhase)v)
                    + SEditorGUILayout.Var("Remain Deliver Days", model.remainDeliverDays).OnValueChanged(v => model.remainDeliverDays = v)
                );
        }

        public static SUIElement RenderGroup(ShopItemModel model, bool fold, UnityAction<bool> setter)
        {
            if (model == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("ShopItemModel", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(model)
                    )
                );
        }
    }
}
