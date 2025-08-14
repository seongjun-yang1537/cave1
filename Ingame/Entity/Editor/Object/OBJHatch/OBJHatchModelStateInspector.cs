using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class OBJHatchModelStateInspector
    {
        public static SUIElement Render(OBJHatchModelState objHatchModelState)
        {
            if (objHatchModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical();
        }

        public static SUIElement RenderGroup(OBJHatchModelState objHatchModelState, bool fold, UnityAction<bool> setter)
        {
            if (objHatchModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("OBJHatch Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(objHatchModelState)
                )
            );
        }
    }
}
