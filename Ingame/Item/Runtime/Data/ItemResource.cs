using System;
using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class ItemResource
    {
        [SerializeField]
        public GameObject prefab;
        [SerializeField]
        public ItemModelData modelData;
        [SerializeField]
        public Sprite iconSprite;

        [SerializeField]
        public Texture2D iconTexture;
    }
}