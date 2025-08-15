using System;
using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class TooltipUISystem : Singleton<TooltipUISystem>
    {
        readonly Dictionary<Type, Stack<ITooltipRenderer>> rendererPools = new();

        ITooltipRenderer _activeRenderer;
        RectTransform _activeRect;
        Type _activeRendererType;

        void LateUpdate()
        {
            if (_activeRenderer == null) return;
            if (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject())
            {
                Hide(null);
                return;
            }

            PositionTooltip(_activeRect);
        }

        ITooltipRenderer GetRenderer(Type contextType)
        {
            if (!rendererPools.TryGetValue(contextType, out var pool))
                pool = rendererPools[contextType] = new();

            if (pool.Count > 0)
                return pool.Pop();

            GameObject prefab = InteractiveUIDB.GetTooltip(contextType);
            if (prefab == null) return null;

            return Instantiate(prefab, transform).GetComponent<ITooltipRenderer>();
        }

        void ReleaseRenderer(Type contextType, ITooltipRenderer renderer)
        {
            if (renderer == null) return;
            if (!rendererPools.TryGetValue(contextType, out var pool))
                pool = rendererPools[contextType] = new();

            renderer.Hide();
            pool.Push(renderer);
        }

        public static void Show(TooltipUIModel context)
        {
            Hide(null);

            var ctxType = context.GetType();
            var renderer = Instance.GetRenderer(ctxType);
            if (renderer == null)
            {
                Debug.LogWarning($"No tooltip prefab for {ctxType}");
                return;
            }

            Instance._activeRenderer = renderer;
            Instance._activeRendererType = ctxType;
            Instance._activeRect = ((MonoBehaviour)renderer).GetComponent<RectTransform>();

            renderer.Render(context);
            renderer.Show();
            Canvas.ForceUpdateCanvases();
            Instance.PositionTooltip(Instance._activeRect);
        }

        public static void Hide(TooltipUIModel _)
        {
            if (Instance._activeRenderer == null) return;

            Instance._activeRenderer.Hide();
            Instance.ReleaseRenderer(Instance._activeRendererType, Instance._activeRenderer);

            Instance._activeRenderer = null;
            Instance._activeRendererType = null;
            Instance._activeRect = null;
        }

        void PositionTooltip(RectTransform tooltipRect)
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 pivot = new(0, 1);
            Vector2 offset = new(15, -15);

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            float tooltipWidth = tooltipRect.rect.width * tooltipRect.lossyScale.x;
            float tooltipHeight = tooltipRect.rect.height * tooltipRect.lossyScale.y;

            if (mousePos.x + tooltipWidth + offset.x > screenWidth) { pivot.x = 1; offset.x = -15; }
            if (mousePos.y - tooltipHeight + offset.y < 0) { pivot.y = 0; offset.y = 15; }

            tooltipRect.pivot = pivot;
            tooltipRect.position = mousePos + offset;
        }
    }
}
