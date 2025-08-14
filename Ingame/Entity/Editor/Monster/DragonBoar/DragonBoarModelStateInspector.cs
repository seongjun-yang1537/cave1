using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class DragonBoarModelStateInspector
    {
        public static SUIElement Render(DragonBoarModelState dragonBoarModelState)
        {
            if (dragonBoarModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("Wander Radius", dragonBoarModelState.wanderRadius)
                    .OnValueChanged(value => dragonBoarModelState.wanderRadius = value)
            );
        }

        public static SUIElement RenderGroup(DragonBoarModelState dragonBoarModelState, bool fold, UnityAction<bool> setter)
        {
            if (dragonBoarModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("DragonBoar Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(dragonBoarModelState)
                )
            );
        }
    }
}
