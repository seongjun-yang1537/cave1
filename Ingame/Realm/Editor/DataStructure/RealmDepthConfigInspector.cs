using System.Collections.Generic;
using Corelib.SUI;
using Ingame;
using UnityEngine;

namespace Realm
{
    public static class RealmDepthConfigInspector
    {
        private class InputState
        {
            public ItemID itemID;
            public int itemCount;
        }

        private static readonly Dictionary<RealmDepthConfig, InputState> inputStates = new();

        public static SUIElement Render(RealmDepthConfig config)
        {
            if (!inputStates.ContainsKey(config))
                inputStates.Add(config, new());

            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Horizontal()
                    .LabelWidth(60f)
                    .Content(
                        SEditorGUILayout.Var("O2 / s", config.oxygenConsumptionPerSecond)
                        .OnValueChanged(value => config.oxygenConsumptionPerSecond = value)
                    )
                    + SEditorGUILayout.Vertical("box")
                    .Content(
                        SEditorGUILayout.Label("Reward Items")
                        .Align(TextAnchor.MiddleCenter)
                        .Bold()
                        + SEditorGUILayout.Separator()
                        + RenderInput(config)
                        + SEditorGUILayout.Separator()
                        + RenderList(config)
                    )
                );
        }

        private static SUIElement RenderInput(RealmDepthConfig config)
        {
            InputState inputState = inputStates[config];

            return SEditorGUILayout.Horizontal()
            .LabelWidth(60f)
            .Content(
                SEditorGUILayout.Label("New Item")
                .Width(64)
                + SEditorGUILayout.Var("", inputState.itemID)
                .OnValueChanged(value => inputState.itemID = (ItemID)value)
                + SEditorGUILayout.Var("", inputState.itemCount)
                .OnValueChanged(value => inputState.itemCount = value)
                + SEditorGUILayout.Button("+")
                .OnClick(() =>
                {
                    if (inputState.itemCount > 0)
                        config.itemModels.Add(ItemModelFactory.Create(new ItemModelState { itemID = inputState.itemID, count = inputState.itemCount }));
                })
                .Width(16)
            );
        }

        private static SUIElement RenderList(RealmDepthConfig config)
        {
            EnsureItemModels(config);

            SUIElement content = SUIElement.Empty();

            for (int i = 0; i < config.itemModels.Count; i++)
            {
                var itemModel = config.itemModels[i];
                GUIContent icon = CreateItemModelGUIContent(itemModel);

                int currentIndex = i;
                content += SEditorGUILayout.Horizontal()
                .LabelWidth(60f)
                .Content(
                    SEditorGUILayout.Button(icon)
                    .Width(32)
                    // + SEditorGUILayout.Var("", itemModel.itemID)
                    //     .OnValueChanged(value => itemModel.itemID = (ItemID)value)
                    + SEditorGUILayout.Var("", itemModel.count)
                        .OnValueChanged(value => itemModel.count = value)
                    + SEditorGUILayout.Button("-")
                        .Width(16)
                        .OnClick(() => config.itemModels.RemoveAt(currentIndex))
                );
            }
            return content;
        }

        private static void EnsureItemModels(RealmDepthConfig config)
        {
            if (config.itemModels == null)
                config.itemModels = new();
        }

        private static GUIContent CreateItemModelGUIContent(ItemModel itemModel)
        {
            if (itemModel == null || itemModel.IsEmpty)
                return new GUIContent();

            Texture2D icon = ItemDB.GetEditorIconTexture(itemModel.itemID);
            string label = itemModel.count > 1 ? $"x{itemModel.count}" : "";
            string tooltip = "";

            return new GUIContent(icon, $"{tooltip} {label}");
        }
    }
}