using System.Collections.Generic;
using System.Linq;
using Corelib.SUI;
using UnityEditor;
using UnityEngine;

namespace Ingame
{
    [CustomEditor(typeof(StoreItemList))]
    public class StoreItemListEditor : Editor
    {
        StoreItemList script;

        void OnEnable()
        {
            script = (StoreItemList)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SEditorGUI.ChangeCheck(
                script,
                SEditorGUILayout.Vertical()
                .Content(
                    RenderItems()
                    + SEditorGUILayout.Button("Add Item")
                        .OnClick(() => script.items.Add(new StoreItemList.StoreItem()))
                )
            )
            .Render();

            serializedObject.ApplyModifiedProperties();
        }

        private SUIElement RenderItems()
        {
            if (script.items == null) return SEditorGUILayout.Label("Items is null");

            var elements = new List<SUIElement>();
            for (int i = 0; i < script.items.Count; i++)
            {
                int index = i;
                var item = script.items[i];
                Texture2D icon = ItemDB.GetEditorIconTexture(item.itemID);

                elements.Add(
                    SEditorGUILayout.Horizontal("box")
                    .Content(
                        SEditorGUILayout.Action(() =>
                        {
                            GUILayout.Box(icon != null ? icon : Texture2D.whiteTexture,
                                GUILayout.Width(32), GUILayout.Height(32));
                        })
                        + SEditorGUILayout.Var("Item", item.itemID)
                            .OnValueChanged(v => item.itemID = (ItemID)v)
                        + SEditorGUILayout.Var("Count", item.count)
                            .OnValueChanged(v => item.count = v)
                        + SEditorGUILayout.Var("Price", item.price)
                            .OnValueChanged(v => item.price = v)
                        + SEditorGUILayout.Button("-")
                            .Width(20)
                            .OnClick(() => script.items.RemoveAt(index))
                    )
                );
            }

            var container = SEditorGUILayout.Vertical();
            if (elements.Count > 0)
                container.Content(elements.Aggregate((a, b) => a + b));
            return container;
        }
    }
}

