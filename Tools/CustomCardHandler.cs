using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Tools
{
    [HarmonyPatch]
    public static class CustomCardHandler
    {
        private static readonly Dictionary<CardType, Action<ZoneBGDataBaseSO, CardInfo>> generators = [];

        [HarmonyPatch(typeof(ZoneBGDataBaseSO), nameof(ZoneBGDataBaseSO.TryGenerateNewCard))]
        [HarmonyPrefix]
        private static bool TryGenerateCustomCard(ZoneBGDataBaseSO __instance, CardInfo info)
        {
            if (!generators.TryGetValue(info.cardType, out var generator))
                return true;

            generator?.Invoke(__instance, info);
            return false;
        }

        public static void AddCardGenerator(CardType type, Action<ZoneBGDataBaseSO, CardInfo> generator)
        {
            generators[type] = generator;
        }
    }
}
