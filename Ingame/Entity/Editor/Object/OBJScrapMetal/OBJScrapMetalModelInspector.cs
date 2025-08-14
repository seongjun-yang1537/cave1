using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class OBJScrapMetalModelInspector
    {
        public static SUIElement Render(OBJScrapMetalModel objScrapMetalModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                OBJScrapMetalModelDataInspector.Render(objScrapMetalModel.Data)
            );
        }

        public static SUIElement RenderGroup(OBJScrapMetalModel objScrapMetalModel, bool fold, UnityAction<bool> setter)
        {
            if (objScrapMetalModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("OBJScrapMetal Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(objScrapMetalModel)
                )
            );
        }
    }
}