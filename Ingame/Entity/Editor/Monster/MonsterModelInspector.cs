using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class MonsterModelInspector
    {
        public static SUIElement Render(MonsterModel monsterModel)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                MonsterModelDataInspector.Render(monsterModel.Data)
            );
        }

        public static SUIElement RenderGroup(MonsterModel monsterModel, bool fold, UnityAction<bool> setter)
        {
            if (monsterModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup("Monster Model", fold)
                .OnValueChanged(setter)
                .Content(
                    Render(monsterModel)
                )
            );
        }
    }
}