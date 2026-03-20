using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Enemies
{
    [HarmonyPatch]
    public static class BlueyEncounters
    {
        public const string SepulchreBundleID = "H_Zone03_Sepulchre_Hard_EnemyBundle";
        public static RandomEnemyBundleSO BlueyBundle;

        public static void Init()
        {
            PortalSignAdder.AddSign(SignTypeE.CadaverSynod, LoadSprite("BlueyOW", new(0.5f, 0f)));

            BlueyBundle = CreateScriptable<RandomEnemyBundleSO>();
            BlueyBundle.name = GetID("H_Zone03_CadaverSynod_Hard_EnemyBundle");
            BlueyBundle._musicEventReference = "event:/Music/Mx_Sepulchre";
            BlueyBundle._roarReference = new("event:/Characters/Enemies/Sepulcre/CHR_ENM_Sepulcre_Roar");
            BlueyBundle._bundleSignType = SignTypeE.CadaverSynod;

            BlueyBundle._enemyBundles =
            [
                new()
                {
                    _enemyNames = [GetID("CadaverSynod_EN"), "GigglingMinister_EN", "SkinningHomunculus_EN"]
                },
                new()
                {
                    _enemyNames = [GetID("CadaverSynod_EN"), "InHisImage_EN", "InHerImage_EN", "GigglingMinister_EN"]
                },
                new()
                {
                    _enemyNames = [GetID("CadaverSynod_EN"), "SkinningHomunculus_EN", "ChoirBoy_EN"]
                }
            ];
        }

        [HarmonyPatch(typeof(RandomEnemyBundleSO), nameof(RandomEnemyBundleSO.GetEnemyBundle))]
        [HarmonyPrefix]
        public static bool MaybeReplaceSepulchre(RandomEnemyBundleSO __instance, ref EnemyCombatBundle __result, BundleDifficulty bundleDifficulty, string roomPrefabName)
        {
            if (__instance.name != SepulchreBundleID)
                return true;

            if (BlueyBundle == null)
                return true;

            var now = DateTime.Now;

            // sepulchre
            if (now.Day % 2 == 1)
                return true;

            // bluey
            __result = BlueyBundle.GetEnemyBundle(bundleDifficulty, roomPrefabName);
            return false;
        }
    }
}
