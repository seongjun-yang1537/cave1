using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class RuptureBlastModelInspector
    {
        public static SUIElement Render(RuptureBlastModel ruptureBlastModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                RuptureBlastModelDataInspector.Render(ruptureBlastModel.Data)
            );
        }

        public static SUIElement RenderGroup(RuptureBlastModel ruptureBlastModel, bool fold, UnityAction<bool> setter)
        {
            if (ruptureBlastModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("RuptureBlast Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(ruptureBlastModel)
                )
            );
        }
    }
}