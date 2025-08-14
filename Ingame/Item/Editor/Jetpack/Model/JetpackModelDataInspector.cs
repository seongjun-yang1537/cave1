using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class JetpackModelDataInspector
    {
        static bool foldBaseStat;

        public static SUIElement Render(JetpackModelData data)
        {
            if (data == null) return SUIElement.Empty();
            if (data.baseStat == null)
                data.baseStat = new();

            return SEditorGUILayout.Vertical()
            .Content(
                JetpackStatInspector.RenderGroup(data.baseStat, foldBaseStat, v => foldBaseStat = v)
            );
        }

        public static SUIElement RenderGroup(JetpackModelData data, bool fold, UnityAction<bool> setter)
        {
            if (data == null) return SUIElement.Empty();
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("JetpackModelData", fold)
                    .OnValueChanged(setter)
                    .Content(
                        Render(data)
                    )
                );
        }
    }
}
