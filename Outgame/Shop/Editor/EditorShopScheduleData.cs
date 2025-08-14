using Corelib.SUI;
using UnityEditor;

namespace Outgame
{
    [CustomEditor(typeof(ShopScheduleData))]
    public class EditorShopScheduleData : Editor
    {
        ShopScheduleData script;
        ShopItemModelData newFixed;
        ShopItemModelData newRandom;
        void OnEnable()
        {
            script = (ShopScheduleData)target;
            if (script.fixedItems == null) script.fixedItems = new();
            if (script.randomItems == null) script.randomItems = new();
        }
        public override void OnInspectorGUI()
        {
            SEditorGUI.ChangeCheck(
                script,
                SEditorGUILayout.Vertical()
                .Content(
                    RenderFixed()
                    + SEditorGUILayout.Separator()
                    + RenderRandom()
                )
            ).Render();
        }
        SUIElement RenderFixed()
        {
            SUIElement elements = SUIElement.Empty();
            elements += SEditorGUILayout.Label("Fixed Items");
            for (int i = 0; i < script.fixedItems.Count; i++)
            {
                int index = i;
                var item = script.fixedItems[i];
                elements += SEditorGUILayout.Horizontal()
                .Content(
                    SEditorGUILayout.Var("", item)
                    .OnValueChanged(v => script.fixedItems[index] = (ShopItemModelData)v)
                    + SEditorGUILayout.Button("-").Width(20)
                    .OnClick(() => script.fixedItems.RemoveAt(index))
                );
            }
            elements += SEditorGUILayout.Horizontal()
            .Content(
                SEditorGUILayout.Var("", newFixed)
                .OnValueChanged(v => newFixed = (ShopItemModelData)v)
                + SEditorGUILayout.Button("+").Width(20)
                .OnClick(() =>
                {
                    if (newFixed != null)
                    {
                        script.fixedItems.Add(newFixed);
                        newFixed = null;
                    }
                })
            );
            return SEditorGUILayout.Vertical().Content(elements);
        }
        SUIElement RenderRandom()
        {
            SUIElement elements = SUIElement.Empty();
            elements += SEditorGUILayout.Label("Random Items");
            for (int i = 0; i < script.randomItems.Count; i++)
            {
                int index = i;
                var item = script.randomItems[i];
                elements += SEditorGUILayout.Horizontal()
                .Content(
                    SEditorGUILayout.Var("", item)
                    .OnValueChanged(v => script.randomItems[index] = (ShopItemModelData)v)
                    + SEditorGUILayout.Button("-").Width(20)
                    .OnClick(() => script.randomItems.RemoveAt(index))
                );
            }
            elements += SEditorGUILayout.Horizontal()
            .Content(
                SEditorGUILayout.Var("", newRandom)
                .OnValueChanged(v => newRandom = (ShopItemModelData)v)
                + SEditorGUILayout.Button("+").Width(20)
                .OnClick(() =>
                {
                    if (newRandom != null)
                    {
                        script.randomItems.Add(newRandom);
                        newRandom = null;
                    }
                })
            );
            return SEditorGUILayout.Vertical().Content(elements);
        }
    }
}
