using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class CreeplingerModelStateInspector
    {
        public static SUIElement Render(CreeplingerModelState creeplingerModelState)
        {
            if (creeplingerModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical();
        }

        public static SUIElement RenderGroup(CreeplingerModelState creeplingerModelState, bool fold, UnityAction<bool> setter)
        {
            if (creeplingerModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Creeplinger Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(creeplingerModelState)
                )
            );
        }
    }
}
