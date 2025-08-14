using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class EnvironmentModelStateInspector
    {
        public static SUIElement Render(EnvironmentModelState environmentModelState)
        {
            if (environmentModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical();
        }

        public static SUIElement RenderGroup(EnvironmentModelState environmentModelState, bool fold, UnityAction<bool> setter)
        {
            if (environmentModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Environment Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(environmentModelState)
                )
            );
        }
    }
}
