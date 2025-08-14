using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class OreModelStateInspector
    {
        public static SUIElement Render(OreModelState oreModelState)
        {
            if (oreModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Var("Drop Item ID", oreModelState.dropItemID)
                    .OnValueChanged(value => oreModelState.dropItemID = (ItemID)value)
                + SEditorGUILayout.Horizontal()
                .Content(
                    SEditorGUILayout.Label("Drop Item Count Range")
                    + SEditorGUILayout.Var("", oreModelState.dropItemCountRange.Min)
                        .OnValueChanged(value => oreModelState.dropItemCountRange.Min = value)
                        .Width(64)
                    + SEditorGUILayout.Label("~")
                        .Width(8)
                    + SEditorGUILayout.Var("", oreModelState.dropItemCountRange.Max)
                        .OnValueChanged(value => oreModelState.dropItemCountRange.Max = value)
                        .Width(64)
                )
            );
        }

        public static SUIElement RenderGroup(OreModelState oreModelState, bool fold, UnityAction<bool> setter)
        {
            if (oreModelState == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Ore Model State", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(oreModelState)
                )
            );
        }
    }
}
