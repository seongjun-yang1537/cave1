using System;

namespace Ingame
{
    [Serializable]
    public class OBJDeliveryBoxModelState : EntityModelState
    {
        public override Type TargetType => typeof(OBJDeliveryBoxModel);

        public ItemModelState itemModelState;
    }
}

