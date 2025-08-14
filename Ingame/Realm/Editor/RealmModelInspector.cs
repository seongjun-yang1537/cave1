using Corelib.SUI;
using UnityEngine.Events;

namespace Realm
{
    public static class RealmModelInspector
    {
        public static SUIElement Render(RealmModel realmModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                RealmModelDataInspector.Render(realmModel.Data)
                + SEditorGUILayout.Var("Depth", realmModel.depth)
                .OnValueChanged(value => realmModel.SetDepth(value))
            // + SEditorGUILayout.Var("Config Table", realmModel.configTable)
            // .OnValueChanged(value => realmModel.configTable = value as RealmConfigTable)
            );
        }

        public static SUIElement RenderGroup(RealmModel entitymodel, bool fold, UnityAction<bool> setter)
        {
            if (entitymodel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Realm Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(entitymodel)
                )
            );
        }
    }
}