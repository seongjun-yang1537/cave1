using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class KnockerModelStateInspector
    {
        public static SUIElement Render(KnockerModelState knockerModelState)
        {
            if (knockerModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical();
        }

        public static SUIElement RenderGroup(KnockerModelState knockerModelState, bool fold, UnityAction<bool> setter)
        {
            if (knockerModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Knocker Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(knockerModelState)
                )
            );
        }
    }
}
