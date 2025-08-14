using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class CreeplingerModelInspector
    {
        public static SUIElement Render(CreeplingerModel creeplingerModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                CreeplingerModelDataInspector.Render(creeplingerModel.Data)
            );
        }

        public static SUIElement RenderGroup(CreeplingerModel creeplingerModel, bool fold, UnityAction<bool> setter)
        {
            if (creeplingerModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Creeplinger Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(creeplingerModel)
                )
            );
        }
    }
}