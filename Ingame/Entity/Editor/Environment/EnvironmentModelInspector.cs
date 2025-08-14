using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class EnvironmentModelInspector
    {
        public static SUIElement Render(EnvironmentModel environmentModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                EnvironmentModelDataInspector.Render(environmentModel.Data)
            );
        }

        public static SUIElement RenderGroup(EnvironmentModel environmentModel, bool fold, UnityAction<bool> setter)
        {
            if (environmentModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Environment Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(environmentModel)
                )
            );
        }
    }
}