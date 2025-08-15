using System;
using Corelib.Utils;
using Ingame;
using UnityEngine;

namespace Outgame
{
    [CreateAssetMenu(fileName = "New Shop Item Model Data", menuName = "Game/Shop/Item Model Data")]

    public class ShopItemModelData : ScriptableObject
    {
        public ItemModelData itemModelData;

        [SerializeField]
        public IntRange deliverDurationRange;

        public int price;
        public int count;
    }
}