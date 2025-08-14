using Corelib.SUI;
using UnityEngine.Events;
using Ingame;
using Core;

namespace Realm
{
    public static class RealmModelDataInspector
    {
        public static SUIElement Render(RealmModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Var("configTable", data.configTable).OnValueChanged(v => data.configTable = v)
                );
        }

        public static SUIElement RenderGroup(RealmModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("RealmModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}

