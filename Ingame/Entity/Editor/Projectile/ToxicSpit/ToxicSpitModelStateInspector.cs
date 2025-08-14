using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class ToxicSpitModelStateInspector
    {
        public static SUIElement Render(ToxicSpitModelState toxicSpitModelState)
        {
            if (toxicSpitModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical();
        }

        public static SUIElement RenderGroup(ToxicSpitModelState toxicSpitModelState, bool fold, UnityAction<bool> setter)
        {
            if (toxicSpitModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("ToxicSpit Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(toxicSpitModelState)
                )
            );
        }
    }
}
