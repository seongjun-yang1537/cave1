using System;
using System.Collections.Generic;
using Corelib.Utils;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ContextUISystem : Singleton<ContextUISystem>
    {
        readonly Dictionary<Type, Stack<IContextUIRenderer>> rendererPools = new();

        IContextUIRenderer activeRenderer;
        RectTransform activeRect;
        Type activeRendererType;

        private void LateUpdate()
        {
            if (activeRenderer == null) return;

            if (Input.anyKeyDown && !Input.GetMouseButtonDown(1))
            {
                if (!RectTransformUtility.RectangleContainsScreenPoint(activeRect, Input.mousePosition, null))
                {
                    Hide(null);
                }
            }
        }

        IContextUIRenderer GetRenderer(Type contextType)
        {
            if (!rendererPools.TryGetValue(contextType, out var pool))
                pool = rendererPools[contextType] = new();
            if (pool.Count > 0)
                return pool.Pop();
            GameObject prefab = InteractiveUIDB.GetContext(contextType);
            if (prefab == null) return null;
            return Instantiate(prefab, transform).GetComponent<IContextUIRenderer>();
        }

        void ReleaseRenderer(Type contextType, IContextUIRenderer renderer)
        {
            if (renderer == null) return;
            if (!rendererPools.TryGetValue(contextType, out var pool))
                pool = rendererPools[contextType] = new();
            renderer.Hide();
            pool.Push(renderer);
        }

        public static void Show(ContextUIModel context)
        {
            Hide(null);
            var ctxType = context.GetType();
            var renderer = Instance.GetRenderer(ctxType);
            if (renderer == null)
            {
                Debug.LogWarning($"No context ui prefab for {ctxType}");
                return;
            }
            Instance.activeRenderer = renderer;
            Instance.activeRendererType = ctxType;
            Instance.activeRect = ((MonoBehaviour)renderer).GetComponent<RectTransform>();
            renderer.Render(context);
            renderer.Show();
            Canvas.ForceUpdateCanvases();
            Instance.Position(Instance.activeRect);
        }

        public static void Hide(ContextUIModel _)
        {
            if (Instance.activeRenderer == null) return;
            Instance.activeRenderer.Hide();
            Instance.ReleaseRenderer(Instance.activeRendererType, Instance.activeRenderer);
            Instance.activeRenderer = null;
            Instance.activeRendererType = null;
            Instance.activeRect = null;
        }

        void Position(RectTransform rect)
        {
            Vector2 mousePos = Input.mousePosition;
            rect.pivot = new Vector2(0, 1);
            rect.position = mousePos;
        }
    }
}
