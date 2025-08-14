using Corelib.SUI;
using UnityEngine.Events;
using Core;
using Corelib.Utils;

namespace Ingame
{
    public static class OreModelDataInspector
    {
        public static SUIElement Render(OreModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Var("dropItemID", data.dropItemID).OnValueChanged(v => data.dropItemID = v)
                    + SEditorGUILayout.Var("dropItemCountRange", data.dropItemCountRange).OnValueChanged(v => data.dropItemCountRange = v)
                );
        }

        public static SUIElement RenderGroup(OreModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("OreModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}
