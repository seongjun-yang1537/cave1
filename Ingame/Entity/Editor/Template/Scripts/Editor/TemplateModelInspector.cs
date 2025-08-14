using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class TemplateModelInspector
    {
        public static SUIElement Render(TemplateModel templateModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
            );
        }

        public static SUIElement RenderGroup(TemplateModel templateModel, bool fold, UnityAction<bool> setter)
        {
            if (templateModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Template Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(templateModel)
                )
            );
        }
    }
}