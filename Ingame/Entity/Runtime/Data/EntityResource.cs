using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class EntityResource
    {
        [SerializeField]
        public GameObject prefab;
        [SerializeReference, SubclassSelector]
        public EntityModelData modelData;
    }
}
