using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class NPCModelDataInspector
    {
        public static SUIElement Render(NPCModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical();
        }

        public static SUIElement RenderGroup(NPCModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("NPCModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}
