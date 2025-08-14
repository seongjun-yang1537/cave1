using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class CockroachModelInspector
    {
        public static SUIElement Render(CockroachModel cockroachModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                CockroachModelDataInspector.Render(cockroachModel.Data)
            );
        }

        public static SUIElement RenderGroup(CockroachModel cockroachModel, bool fold, UnityAction<bool> setter)
        {
            if (cockroachModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Cockroach Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(cockroachModel)
                )
            );
        }
    }
}