using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class RuptureBlastModelDataInspector
    {
        public static SUIElement Render(RuptureBlastModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical();
        }

        public static SUIElement RenderGroup(RuptureBlastModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("RuptureBlastModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}
