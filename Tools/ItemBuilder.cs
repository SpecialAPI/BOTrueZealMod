using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Tools
{
    [HarmonyPatch]
    public static class ItemBuilder
    {
        [HarmonyPatch(typeof(ItemPoolDataBaseSO), nameof(ItemPoolDataBaseSO.ShopPool), MethodType.Getter)]
        [HarmonyPatch(typeof(ItemPoolDataBaseSO), nameof(ItemPoolDataBaseSO.TreasurePool), MethodType.Getter)]
        [HarmonyPrefix]
        public static void AddItemsToPool(ItemPoolDataBaseSO __instance)
        {
            if (itemPool == null)
            {
                itemPool = __instance;

                itemPool._TreasurePool = itemPool._TreasurePool.Concat(treasuresToAdd).ToArray();
                itemPool._ShopPool = itemPool._ShopPool.Concat(shopItemsToAdd).ToArray();

                treasuresToAdd.Clear();
                shopItemsToAdd.Clear();
            }
        }

        public static ItemPoolDataBaseSO itemPool;
        private static readonly List<string> shopItemsToAdd = new();
        private static readonly List<string> treasuresToAdd = new();
    }
}
