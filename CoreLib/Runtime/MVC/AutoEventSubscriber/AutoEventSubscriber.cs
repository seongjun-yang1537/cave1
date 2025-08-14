using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
using System.Collections.Generic;
using System;
using Corelib.Utils;
using System.Linq;

namespace Corelib.Utils
{
    public class AutoEventSubscriber : ILifecycleInjectable
    {
        private readonly MonoBehaviour mono;
        private readonly Dictionary<Delegate, (UnityEventBase, MethodInfo)> _subscribedActions = new();

        public AutoEventSubscriber(MonoBehaviour mono)
        {
            this.mono = mono;
        }

        public void OnEnable()
        {
            var targetType = mono.GetType();
            var context = mono as UnityEngine.Object;

            foreach (var method in targetType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                var attributes = method.GetCustomAttributes<AutoSubscribeAttribute>(true);
                foreach (var attribute in attributes)
                {
                    if (!TryGetUnityEvent(targetType, mono, attribute.EventName, out var unityEvent))
                    {
                        Debug.LogError($"[AutoSubscriber] Event '{attribute.EventName}' not found or not a UnityEvent on {targetType.Name}.", context);
                        continue;
                    }

                    try
                    {
                        var action = CreateAction(method, unityEvent);
                        if (action != null)
                        {
                            unityEvent.GetType().GetMethod("AddListener")?.Invoke(unityEvent, new object[] { action });
                            _subscribedActions[action] = (unityEvent, method);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[AutoSubscriber] Failed to subscribe '{method.Name}' to '{attribute.EventName}'. Error: {e.Message}", context);
                    }
                }
            }
        }

        public void OnDisable()
        {
            foreach (var (action, (unityEvent, _)) in _subscribedActions)
            {
                if (unityEvent != null)
                {
                    unityEvent.GetType().GetMethod("RemoveListener")?.Invoke(unityEvent, new object[] { action });
                }
            }
            _subscribedActions.Clear();
        }

        private Delegate CreateAction(MethodInfo method, UnityEventBase unityEvent)
        {
            var delegateType = unityEvent.GetType().GetMethod("AddListener").GetParameters()[0].ParameterType;
            return Delegate.CreateDelegate(delegateType, mono, method);
        }

        private bool TryGetUnityEvent(Type targetType, object target, string name, out UnityEventBase unityEvent)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            // 1) Field
            var field = targetType.GetField(name, flags);
            if (field != null)
            {
                var val = field.GetValue(target);
                if (val is UnityEventBase ev) { unityEvent = ev; return true; }
            }

            // 2) Property
            var prop = targetType.GetProperty(name, flags);
            if (prop != null && prop.CanRead && prop.GetIndexParameters().Length == 0)
            {
                var val = prop.GetValue(target);
                if (val is UnityEventBase ev)
                {
                    unityEvent = ev;
                    return true;
                }

                // 필요하면: null일 때 기본 생성해 세팅 (set 가능 + 기본 생성자 존재)
                if (val == null && prop.CanWrite && typeof(UnityEventBase).IsAssignableFrom(prop.PropertyType))
                {
                    var ctor = prop.PropertyType.GetConstructor(Type.EmptyTypes);
                    if (ctor != null)
                    {
                        var inst = (UnityEventBase)Activator.CreateInstance(prop.PropertyType);
                        prop.SetValue(target, inst);
                        unityEvent = inst;
                        return true;
                    }
                }
            }

            unityEvent = null;
            return false;
        }
    }
}
