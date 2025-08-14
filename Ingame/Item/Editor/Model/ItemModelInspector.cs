using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class ItemModelInspector
    {
        static bool foldData;
        public static SUIElement Render(ItemModel itemModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                ItemModelDataInspector.RenderGroup(itemModel.Data, foldData, v => foldData = v)
                + SEditorGUILayout.Var("Count", itemModel.count)
                .OnValueChanged(value => itemModel.count = value)
            );
        }

        public static SUIElement RenderGroup(ItemModel itemModel, bool fold, UnityAction<bool> setter)
        {
            if (itemModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Item Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(itemModel)
                )
            );
        }
    }
}