using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class AntModelStateInspector
    {
        public static SUIElement Render(AntModelState antModelState)
        {
            if (antModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical();
        }

        public static SUIElement RenderGroup(AntModelState antModelState, bool fold, UnityAction<bool> setter)
        {
            if (antModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Ant Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(antModelState)
                )
            );
        }
    }
}
