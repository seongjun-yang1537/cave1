using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class OreModelInspector
    {
        public static SUIElement Render(OreModel oreModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                OreModelDataInspector.Render(oreModel.Data)
                + SEditorGUILayout.Horizontal()
                .Content(
                    SEditorGUILayout.Label("Drop Item Count Range")
                    + SEditorGUILayout.Var("", oreModel.dropItemCountRange.Min)
                    .OnValueChanged(value => oreModel.dropItemCountRange.Min = value)
                    .Width(64)
                    + SEditorGUILayout.Label("~")
                    .Width(8)
                    + SEditorGUILayout.Var("", oreModel.dropItemCountRange.Max)
                    .OnValueChanged(value => oreModel.dropItemCountRange.Max = value)
                    .Width(64)
                )
            );
        }

        public static SUIElement RenderGroup(OreModel oreModel, bool fold, UnityAction<bool> setter)
        {
            if (oreModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Ore Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(oreModel)
                )
            );
        }
    }
}