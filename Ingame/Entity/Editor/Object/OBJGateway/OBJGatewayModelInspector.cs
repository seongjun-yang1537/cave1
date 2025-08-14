using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class OBJGatewayModelInspector
    {
        public static SUIElement Render(OBJGatewayModel objGatewayModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                OBJGatewayModelDataInspector.Render(objGatewayModel.Data)
            );
        }

        public static SUIElement RenderGroup(OBJGatewayModel objGatewayModel, bool fold, UnityAction<bool> setter)
        {
            if (objGatewayModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("OBJGateway Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(objGatewayModel)
                )
            );
        }
    }
}