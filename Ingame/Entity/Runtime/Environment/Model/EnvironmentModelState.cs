using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class EnvironmentModelState : EntityModelState
    {
        public override Type TargetType => typeof(EnvironmentModel);
    }
}
