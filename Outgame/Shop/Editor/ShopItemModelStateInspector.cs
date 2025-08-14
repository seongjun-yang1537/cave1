using Corelib.SUI;
using UnityEngine.Events;
using Ingame;

namespace Outgame
{
    public static class ShopItemModelStateInspector
    {
        static bool foldItemModelState;

        public static SUIElement Render(ShopItemModelState state)
        {
            if (state == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    ItemModelStateInspector.RenderGroup(state.itemModel, foldItemModelState, v => foldItemModelState = v)
                    + SEditorGUILayout.Var("Phase", state.phase).OnValueChanged(v => state.phase = (ShopItemPhase)v)
                    + SEditorGUILayout.Var("Remain Deliver Days", state.remainDeliverDays).OnValueChanged(v => state.remainDeliverDays = v)
                );
        }

        public static SUIElement RenderGroup(ShopItemModelState state, bool fold, UnityAction<bool> setter)
        {
            if (state == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("ShopItemModelState", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(state)
                    )
                );
        }
    }
}
