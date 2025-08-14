using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class OBJGatewayModelDataInspector
    {
        public static SUIElement Render(OBJGatewayModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical();
        }

        public static SUIElement RenderGroup(OBJGatewayModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("OBJGatewayModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}

