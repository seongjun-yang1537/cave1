using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class OBJDeliveryBoxModelInspector
    {
        static bool foldItemModel;
        public static SUIElement Render(OBJDeliveryBoxModel objDeliveryBoxModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                ItemModelInspector.RenderGroup(objDeliveryBoxModel.itemModel, foldItemModel, v => foldItemModel = v)
            );
        }

        public static SUIElement RenderGroup(OBJDeliveryBoxModel objDeliveryBoxModel, bool fold, UnityAction<bool> setter)
        {
            if (objDeliveryBoxModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("OBJDeliveryBox Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(objDeliveryBoxModel)
                )
            );
        }
    }
}