using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class MerchantNPCModelInspector
    {
        public static SUIElement Render(MerchantNPCModel merchantNPCModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
            );
        }

        public static SUIElement RenderGroup(MerchantNPCModel merchantNPCModel, bool fold, UnityAction<bool> setter)
        {
            if (merchantNPCModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("MerchantNPC Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(merchantNPCModel)
                )
            );
        }
    }
}