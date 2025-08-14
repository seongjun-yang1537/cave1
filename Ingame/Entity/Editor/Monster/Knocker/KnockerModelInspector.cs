using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class KnockerModelInspector
    {
        public static SUIElement Render(KnockerModel knockerModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                KnockerModelDataInspector.Render(knockerModel.Data)
            );
        }

        public static SUIElement RenderGroup(KnockerModel knockerModel, bool fold, UnityAction<bool> setter)
        {
            if (knockerModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Knocker Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(knockerModel)
                )
            );
        }
    }
}