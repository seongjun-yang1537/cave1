using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class OBJScrapMetalModelStateInspector
    {
        public static SUIElement Render(OBJScrapMetalModelState objScrapMetalModelState)
        {
            if (objScrapMetalModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical();
        }

        public static SUIElement RenderGroup(OBJScrapMetalModelState objScrapMetalModelState, bool fold, UnityAction<bool> setter)
        {
            if (objScrapMetalModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("OBJScrapMetal Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(objScrapMetalModelState)
                )
            );
        }
    }
}
