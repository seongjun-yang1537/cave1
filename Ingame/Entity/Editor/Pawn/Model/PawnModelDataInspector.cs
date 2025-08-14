using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class PawnModelDataInspector
    {
        static bool foldPawnStat;
        static bool foldPhysics;

        public static SUIElement Render(PawnModelData data)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    PawnStatInspector.RenderGroup(data.pawnBaseStat, foldPawnStat, v => foldPawnStat = v)
                    + SEditorGUILayout.Vertical("box")
                    .Content(
                        SEditorGUILayout.Object("Physics Setting", data.physicsSetting, typeof(PawnPhysicsSetting))
                        .OnValueChanged(value => data.physicsSetting = value as PawnPhysicsSetting)
                        + PawnPhysicsSettingInspector.RenderGroup(data.physicsSetting, foldPhysics, v => foldPhysics = v)
                    )
                );
        }

        public static SUIElement RenderGroup(PawnModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("PawnModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}
