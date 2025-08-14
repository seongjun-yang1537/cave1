using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class OBJScrapMetalModelState : EntityModelState
    {
        public override Type TargetType => typeof(OBJScrapMetalModel);
    }
}
