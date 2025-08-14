using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class CockroachModelStateInspector
    {
        public static SUIElement Render(CockroachModelState cockroachModelState)
        {
            if (cockroachModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical();
        }

        public static SUIElement RenderGroup(CockroachModelState cockroachModelState, bool fold, UnityAction<bool> setter)
        {
            if (cockroachModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Cockroach Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(cockroachModelState)
                )
            );
        }
    }
}
