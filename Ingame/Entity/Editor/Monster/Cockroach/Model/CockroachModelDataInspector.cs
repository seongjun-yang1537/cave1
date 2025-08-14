using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class CockroachModelDataInspector
    {
        public static SUIElement Render(CockroachModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical();
        }

        public static SUIElement RenderGroup(CockroachModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("CockroachModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}

