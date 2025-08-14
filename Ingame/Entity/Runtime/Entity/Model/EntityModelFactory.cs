using System;
using System.Reflection;
using Corelib.Utils;

namespace Ingame
{
    public static class EntityModelFactory
    {
        static Type GetDataType(Type modelType)
        {
            var name = $"{modelType.Namespace}.{modelType.Name}Data";
            var t = modelType.Assembly.GetType(name);
            if (t != null && typeof(EntityModelData).IsAssignableFrom(t))
                return t;
            return typeof(EntityModelData);
        }

        static Type GetStateType(Type modelType)
        {
            var name = $"{modelType.Namespace}.{modelType.Name}State";
            var t = modelType.Assembly.GetType(name);
            if (t != null && typeof(EntityModelState).IsAssignableFrom(t))
                return t;
            return typeof(EntityModelState);
        }

        public static EntityModel Create(EntityType entityType)
        {
            var data = EntityDB.LoadModelData(entityType);
            return Create(data);
        }

        public static EntityModel Create(EntityModelData data)
        {
            var modelType = data.TargetType;
            return CreateInstance(modelType, data, null);
        }

        public static EntityModel Create(EntityModelState state)
        {
            var data = EntityDB.LoadModelData(state.entityType);
            return Create(data, state);
        }

        public static EntityModel Create(EntityModelData data, EntityModelState state)
        {
            var modelType = ExType.LCAType(data.TargetType, state.TargetType);
            return CreateInstance(modelType, data, state);
        }

        static EntityModel CreateInstance(Type modelType, EntityModelData data, EntityModelState state)
        {
            var dataType = GetDataType(modelType);
            if (data != null && !dataType.IsInstanceOfType(data))
                throw new ArgumentException(nameof(data));

            var stateType = GetStateType(modelType);
            if (state != null && !stateType.IsInstanceOfType(state))
                throw new ArgumentException(nameof(state));

            var ctor = modelType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new[] { dataType, stateType }, null);
            return ctor.Invoke(new object[] { data, state }) as EntityModel;
        }
    }
}

