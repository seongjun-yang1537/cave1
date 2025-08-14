using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class OBJScrapMetalModelData : EntityModelData
    {
        public override Type TargetType => typeof(OBJScrapMetalModel);
    }
}

