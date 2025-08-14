using System;

namespace Ingame
{
    public static class EntityModelUtils
    {
        public static EntityModelState CreateState(EntityModelData data)
        {
            if (data == null) return null;
            var t = data.GetType();
            var name = t.FullName.Replace("ModelData", "ModelState");
            var stateType = t.Assembly.GetType(name);
            if (stateType == null) return null;
            return Activator.CreateInstance(stateType) as EntityModelState;
        }
    }
}
