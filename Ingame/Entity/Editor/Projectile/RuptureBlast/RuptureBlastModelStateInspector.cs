using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class RuptureBlastModelStateInspector
    {
        public static SUIElement Render(RuptureBlastModelState ruptureBlastModelState)
        {
            if (ruptureBlastModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical();
        }

        public static SUIElement RenderGroup(RuptureBlastModelState ruptureBlastModelState, bool fold, UnityAction<bool> setter)
        {
            if (ruptureBlastModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("RuptureBlast Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(ruptureBlastModelState)
                )
            );
        }
    }
}
