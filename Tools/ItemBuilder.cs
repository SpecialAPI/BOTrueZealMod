using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Tools
{
    [HarmonyPatch]
    public static class ItemBuilder
    {
        public static ItemPoolDataBaseSO itemPool;
        private static readonly List<string> shopItemsToAdd = [];
        private static readonly List<string> treasuresToAdd = [];

        [HarmonyPatch(typeof(ItemPoolDataBaseSO), $"get_{nameof(ItemPoolDataBaseSO.ShopPool)}")]
        [HarmonyPatch(typeof(ItemPoolDataBaseSO), $"get_{nameof(ItemPoolDataBaseSO.TreasurePool)}")]
        [HarmonyPatch(typeof(ItemPoolDataBaseSO), nameof(ItemPoolDataBaseSO.InitializePool))]
        [HarmonyPrefix]
        private static void AddItemsToPool(ItemPoolDataBaseSO __instance)
        {
            if (itemPool != null)
                return;

            itemPool = __instance;

            itemPool._TreasurePool = itemPool._TreasurePool.AddRangeToArray([.. treasuresToAdd]);
            itemPool._ShopPool = itemPool._ShopPool.AddRangeToArray([.. shopItemsToAdd]);

            treasuresToAdd.Clear();
            shopItemsToAdd.Clear();
        }
    }
}
