using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class OBJScrapMetalModelDataInspector
    {
        public static SUIElement Render(OBJScrapMetalModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical();
        }

        public static SUIElement RenderGroup(OBJScrapMetalModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("OBJScrapMetalModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}

