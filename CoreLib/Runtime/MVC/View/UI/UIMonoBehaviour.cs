using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using TriInspector;
using Corelib.Utils;
using UnityEngine.Events;

namespace UI
{
    [DeclareBoxGroup("Placeholder")]
    [DeclareBoxGroup("UIMonoBehaviour", Title = "UI MonoBehaviour")]
    public abstract class UIMonoBehaviour : MonoBehaviour
    {
        public UnityEvent onRenderEnd = new();

        [LifecycleInject] protected AutoEventSubscriber autoEventSubscriber;

        protected IUIViewHandler viewHandler;
        protected UIMonoBehaviour parent;

        [Serializable]
        public struct ChildEntry
        {
            [HideInInspector] public string fieldName;
            [ReadOnly] public UIMonoBehaviour ui;
        }

        [Group("UIMonoBehaviour"), ReadOnly]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        [SerializeField] private List<ChildEntry> childUIs = new();

        static readonly Dictionary<Type, FieldInfo[]> fieldCache = new();

        static FieldInfo[] GetCachedFields(Type type)
        {
            if (!fieldCache.TryGetValue(type, out var fields))
            {
                const BindingFlags F = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
                fields = type.GetFields(F);
                fieldCache[type] = fields;
            }
            return fields;
        }

        protected virtual void Awake()
        {
            CacheChildUIs();
            var t = transform.parent;
            if (t != null)
            {
                parent = t.GetComponentInParent<UIMonoBehaviour>();
                if (parent != null && parent.viewHandler != null)
                    BindViewHandler(parent.viewHandler);
            }
            LifecycleInjectionUtil.ConstructLifecycleObjects(this);
        }

        protected virtual void OnEnable() => LifecycleInjectionUtil.OnEnable(this);
        protected virtual void OnDisable() => LifecycleInjectionUtil.OnDisable(this);

        private void CacheChildUIs()
        {
            childUIs.Clear();

            foreach (var field in GetCachedFields(GetType()))
            {
                var t = field.FieldType;

                if (typeof(UIMonoBehaviour).IsAssignableFrom(t))
                {
                    if (field.GetValue(this) is UIMonoBehaviour ui && ui.transform != transform && ui.transform.IsChildOf(transform))
                    {
                        ui.parent = this;
                        if (viewHandler != null)
                            ui.BindViewHandler(viewHandler);
                        childUIs.Add(new ChildEntry { fieldName = field.Name, ui = ui });
                    }
                }
                else if (typeof(IEnumerable).IsAssignableFrom(t))
                {
                    var elem = t.IsArray ? t.GetElementType()
                                         : t.IsGenericType ? t.GetGenericArguments()[0] : null;

                    if (elem != null && typeof(UIMonoBehaviour).IsAssignableFrom(elem) &&
                        field.GetValue(this) is IEnumerable enumerable)
                    {
                        foreach (var obj in enumerable)
                            if (obj is UIMonoBehaviour ui && ui.transform != transform && ui.transform.IsChildOf(transform))
                            {
                                ui.parent = this;
                                if (viewHandler != null)
                                    ui.BindViewHandler(viewHandler);
                                childUIs.Add(new ChildEntry { fieldName = field.Name, ui = ui });
                            }
                    }
                }
            }
        }

#if UNITY_EDITOR
        private void OnValidate() => CacheChildUIs();
#endif

        public IReadOnlyList<UIMonoBehaviour> Children
        {
            get
            {
                var list = new List<UIMonoBehaviour>(childUIs.Count);
                foreach (var e in childUIs) list.Add(e.ui);
                return list;
            }
        }

        public UIMonoBehaviour Parent => parent;

        public virtual void BindViewHandler(IUIViewHandler handler)
        {
            viewHandler = handler;
            foreach (var e in childUIs) e.ui.BindViewHandler(handler);
        }

        [Button("Render")]
        public abstract void Render();
    }
}
