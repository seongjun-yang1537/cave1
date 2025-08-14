using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public static class EntityModelDataDB
    {
        static readonly Dictionary<Type, EntityModelData> _database;

        static EntityModelDataDB()
        {
            _database = new Dictionary<Type, EntityModelData>();
            var table = Resources.Load<EntityModelDataTable>("Ingame/EntityModelDataDB");
            if (table != null)
            {
                foreach (var kvp in table.table)
                {
                    var type = Type.GetType(kvp.Key);
                    if (type != null && kvp.Value != null)
                        _database[type] = kvp.Value;
                }
            }
        }

        public static EntityModelData Find(Type type)
        {
            _database.TryGetValue(type, out var data);
            return data;
        }

        public static T Find<T>() where T : EntityModelData
        {
            _database.TryGetValue(typeof(T), out var data);
            return (T)data;
        }
    }
}
