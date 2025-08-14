using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class NPCModelInspector
    {
        public static SUIElement Render(NPCModel npcModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                NPCModelDataInspector.Render(npcModel.Data)
            );
        }

        public static SUIElement RenderGroup(NPCModel npcModel, bool fold, UnityAction<bool> setter)
        {
            if (npcModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("NPC Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(npcModel)
                )
            );
        }
    }
}