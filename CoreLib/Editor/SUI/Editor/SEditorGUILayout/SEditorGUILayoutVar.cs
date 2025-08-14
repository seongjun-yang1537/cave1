using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UObject = UnityEngine.Object;

namespace Corelib.SUI
{
    static class EnumDefaults
    {
        static readonly Dictionary<Type, Enum> cache = new();
        public static Enum Get(Type type)
        {
            if (!cache.TryGetValue(type, out var value))
            {
                value = (Enum)Enum.GetValues(type).GetValue(0);
                cache[type] = value;
            }
            return value;
        }
    }

    public class SEditorGUILayoutVar<T> : SUIElement
    {
        string label;
        T value;
        UnityAction<T> onValueChanged;
        float? width;
        float? height;

        public SEditorGUILayoutVar(string label, T value)
        {
            this.label = label;
            this.value = value;
        }

        public SEditorGUILayoutVar<T> OnValueChanged(UnityAction<T> onValueChanged)
        {
            this.onValueChanged = onValueChanged;
            return this;
        }

        public SEditorGUILayoutVar<T> Width(float width)
        {
            this.width = width;
            return this;
        }

        public SEditorGUILayoutVar<T> Height(float height)
        {
            this.height = height;
            return this;
        }

        public override void Render()
        {
            var type = typeof(T);
            SUIElement element = SUIElement.Empty();
            if (typeof(UObject).IsAssignableFrom(type))
                element = SEditorGUILayout.Object(label, value as UObject, type).OnValueChanged(v =>
                {
                    value = (T)(object)v;
                    onValueChanged?.Invoke((T)(object)v);
                });
            else if (type.IsEnum)
                element = SEditorGUILayout.Enum(label, value == null ? EnumDefaults.Get(type) : (Enum)(object)value).OnValueChanged(v =>
                {
                    var obj = (T)(object)v;
                    value = obj;
                    onValueChanged?.Invoke(obj);
                });
            else if (type == typeof(int))
            {
                var intElement = SEditorGUILayout.Int(label, Convert.ToInt32(value)).OnValueChanged(v =>
                {
                    value = (T)(object)v;
                    onValueChanged?.Invoke((T)(object)v);
                });
                if (width != null) intElement.Width(width.Value);
                if (height != null) intElement.Height(height.Value);
                element = intElement;
            }
            else if (type == typeof(float))
            {
                var floatElement = SEditorGUILayout.Float(label, Convert.ToSingle(value)).OnValueChanged(v =>
                {
                    value = (T)(object)v;
                    onValueChanged?.Invoke((T)(object)v);
                });
                if (width != null) floatElement.Width(width.Value);
                if (height != null) floatElement.Height(height.Value);
                element = floatElement;
            }
            else if (type == typeof(bool))
            {
                var toggleElement = SEditorGUILayout.Toggle(label, Convert.ToBoolean(value)).OnValueChanged(v =>
                {
                    value = (T)(object)v;
                    onValueChanged?.Invoke((T)(object)v);
                });
                element = toggleElement;
            }
            else if (type == typeof(string))
            {
                var textElement = SEditorGUILayout.Text(label, value as string ?? string.Empty).OnValueChanged(v =>
                {
                    value = (T)(object)v;
                    onValueChanged?.Invoke((T)(object)v);
                });
                if (width != null) textElement.Width(width.Value);
                if (height != null) textElement.Height(height.Value);
                element = textElement;
            }
            else if (type == typeof(Vector3))
            {
                var vector3Element = SEditorGUILayout.Vector3(label, value == null ? default : (Vector3)(object)value).OnValueChanged(v =>
                {
                    value = (T)(object)v;
                    onValueChanged?.Invoke((T)(object)v);
                });
                element = vector3Element;
            }
            else if (type == typeof(Vector3Int))
            {
                var vector3IntElement = SEditorGUILayout.Vector3Int(label, value == null ? default : (Vector3Int)(object)value).OnValueChanged(v =>
                {
                    value = (T)(object)v;
                    onValueChanged?.Invoke((T)(object)v);
                });
                element = vector3IntElement;
            }
            else
                element = SEditorGUILayout.Label($"{label}: Unsupported type '{type.Name}'");
            element.Render();
        }
    }
}
