using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class AntModelInspector
    {
        public static SUIElement Render(AntModel antModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                AntModelDataInspector.Render(antModel.Data)
            );
        }

        public static SUIElement RenderGroup(AntModel antModel, bool fold, UnityAction<bool> setter)
        {
            if (antModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Ant Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(antModel)
                )
            );
        }
    }
}