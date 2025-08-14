using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class ToxicSpitModelDataInspector
    {
        public static SUIElement Render(ToxicSpitModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical();
        }

        public static SUIElement RenderGroup(ToxicSpitModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("ToxicSpitModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}
