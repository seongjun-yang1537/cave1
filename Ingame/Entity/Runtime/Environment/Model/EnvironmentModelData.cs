using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class EnvironmentModelData : EntityModelData
    {
        public override Type TargetType => typeof(EnvironmentModel);
    }
}

