using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class OBJGatewayModelStateInspector
    {
        public static SUIElement Render(OBJGatewayModelState objGatewayModelState)
        {
            if (objGatewayModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical();
        }

        public static SUIElement RenderGroup(OBJGatewayModelState objGatewayModelState, bool fold, UnityAction<bool> setter)
        {
            if (objGatewayModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("OBJGateway Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(objGatewayModelState)
                )
            );
        }
    }
}
