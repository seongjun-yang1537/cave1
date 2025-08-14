using System;
using Corelib.Utils;

namespace Ingame
{
    [Serializable]
    public class EntityModelState : IHasTargetType
    {
        public virtual Type TargetType => typeof(EntityModel);

        public bool isSpanwed;
        public EntityType entityType;
    }
}
