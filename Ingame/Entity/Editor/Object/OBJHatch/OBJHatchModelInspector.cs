using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class OBJHatchModelInspector
    {
        public static SUIElement Render(OBJHatchModel objHatchModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                OBJHatchModelDataInspector.Render(objHatchModel.Data)
            );
        }

        public static SUIElement RenderGroup(OBJHatchModel objHatchModel, bool fold, UnityAction<bool> setter)
        {
            if (objHatchModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("OBJHatch Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(objHatchModel)
                )
            );
        }
    }
}