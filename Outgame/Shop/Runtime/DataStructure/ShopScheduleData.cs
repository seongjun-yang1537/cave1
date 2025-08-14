using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;

namespace Outgame
{
    [CreateAssetMenu(menuName = "Game/Shop/Schedule Data")]
    public class ShopScheduleData : ScriptableObject
    {
        public List<ShopItemModelData> fixedItems;
        public List<ShopItemModelData> randomItems;
        public List<ShopItemModel> Generate(int day, MT19937 rng)
        {
            List<ShopItemModel> result = new List<ShopItemModel>();
            if (fixedItems != null)
            {
                foreach (var item in fixedItems)
                {
                    if (item == null) continue;
                    var model = new ShopItemModel(item);
                    if (model != null) result.Add(model);
                }
            }
            if (randomItems != null)
            {
                foreach (var item in randomItems)
                {
                    if (item == null) continue;
                    var model = new ShopItemModel(item);
                    if (model != null) result.Add(model);
                }
            }
            return result;
        }
    }
}
