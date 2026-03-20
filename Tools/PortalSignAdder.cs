using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Tools
{
    [HarmonyPatch]
    public static class PortalSignAdder
    {
        public static PortalSignsDataBaseSO signs;
        private static readonly List<PortalSignsDataBaseSO.PortalSignIcon> signsToAdd = [];

        public static void AddSign(SignType sign, Sprite icon)
        {
            var portalSign = new PortalSignsDataBaseSO.PortalSignIcon()
            {
                signType = sign,
                signIcon = icon
            };

            if (signs != null)
                signs._otherSigns = signs._otherSigns.AddToArray(portalSign);
            else
                signsToAdd.Add(portalSign);
        }

        [HarmonyPatch(typeof(PortalSignsDataBaseSO), nameof(PortalSignsDataBaseSO.InitializeSignDB))]
        [HarmonyPrefix]
        private static void AddSignsToDatabase(PortalSignsDataBaseSO __instance)
        {
            if (signs != null)
                return;

            signs = __instance;
            signs._otherSigns = signs._otherSigns.AddRangeToArray([..signsToAdd]);

            signsToAdd.Clear();
        }
    }
}
