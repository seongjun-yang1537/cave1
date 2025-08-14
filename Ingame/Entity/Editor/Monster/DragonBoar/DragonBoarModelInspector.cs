using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class DragonBoarModelInspector
    {
        public static SUIElement Render(DragonBoarModel dragonBoarModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                DragonBoarModelDataInspector.Render(dragonBoarModel.Data)
            );
        }

        public static SUIElement RenderGroup(DragonBoarModel dragonBoarModel, bool fold, UnityAction<bool> setter)
        {
            if (dragonBoarModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("DragonBoar Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(dragonBoarModel)
                )
            );
        }
    }
}