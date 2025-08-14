using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class ToxicSpitModelInspector
    {
        public static SUIElement Render(ToxicSpitModel toxicSpitModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                ToxicSpitModelDataInspector.Render(toxicSpitModel.Data)
            );
        }

        public static SUIElement RenderGroup(ToxicSpitModel toxicSpitModel, bool fold, UnityAction<bool> setter)
        {
            if (toxicSpitModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("ToxicSpit Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(toxicSpitModel)
                )
            );
        }
    }
}
