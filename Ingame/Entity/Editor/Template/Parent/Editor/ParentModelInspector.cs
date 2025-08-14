using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class ParentModelInspector
    {
        public static SUIElement Render(ParentModel parentModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
            );
        }

        public static SUIElement RenderGroup(ParentModel parentModel, bool fold, UnityAction<bool> setter)
        {
            if (parentModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Parent Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(parentModel)
                )
            );
        }
    }
}