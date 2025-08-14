using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class AntModelDataInspector
    {
        public static SUIElement Render(AntModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical();
        }

        public static SUIElement RenderGroup(AntModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("AntModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}

