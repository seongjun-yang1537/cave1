using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class RuptureModelInspector
    {
        public static SUIElement Render(RuptureModel ruptureModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                RuptureModelDataInspector.Render(ruptureModel.Data)
                + SEditorGUILayout.Var("Explosion Range", ruptureModel.explosionRange)
                .OnValueChanged(value => ruptureModel.explosionRange = value)
                + SEditorGUILayout.Var("Explosion Fuse Time", ruptureModel.explosionFuseTime)
                .OnValueChanged(value => ruptureModel.explosionFuseTime = value)
            );
        }

        public static SUIElement RenderGroup(RuptureModel ruptureModel, bool fold, UnityAction<bool> setter)
        {
            if (ruptureModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Rupture Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(ruptureModel)
                )
            );
        }
    }
}