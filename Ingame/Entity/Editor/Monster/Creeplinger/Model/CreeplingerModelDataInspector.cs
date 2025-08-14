using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class CreeplingerModelDataInspector
    {
        public static SUIElement Render(CreeplingerModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical();
        }

        public static SUIElement RenderGroup(CreeplingerModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("CreeplingerModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}

