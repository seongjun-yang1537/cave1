using System;
using UnityEngine;

namespace UI
{
    public static class InteractiveUIDB
    {
        const string PATH_TABLE = "UI/InteractiveUITable";
        static InteractiveUITable table;

        static InteractiveUIDB()
        {
            table = Resources.Load<InteractiveUITable>(PATH_TABLE);
        }

        public static GameObject GetTooltip(Type type) => table?.GetTooltip(type);
        public static GameObject GetContext(Type type) => table?.GetContext(type);
    }
}
