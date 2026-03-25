using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Tools
{
    [HarmonyPatch]
    public static class UnlockBuilder
    {
        [HarmonyPatch(typeof(UnlockablesManager), nameof(UnlockablesManager.TryUnlockContent))]
        [HarmonyPostfix]
        private static void TryUnlockCustomAchievement(UnlockablesManager __instance, UnlockableData data)
        {
            if (data is not CustomUnlockableData customData)
                return;

            if (customData.hasCustomAchievementUnlock)
                __instance._game.UnlockCustomAchievement(customData.customAchievementID);
        }

        public static CustomUnlockableData NewUnlock(UnlockableID id)
        {
            return new CustomUnlockableData()
            {
                id = id
            };
        }

        public static T SetAchievement<T>(this T u, CustomAchievement achievement) where T : CustomUnlockableData
        {
            u.hasCustomAchievementUnlock = true;
            u.customAchievementID = achievement.id;

            return u;
        }

        public static T SetItems<T>(this T u, string[] items) where T : UnlockableData
        {
            u.hasItemUnlock = true;
            u.items = items;

            return u;
        }

        public static T SetCharacter<T>(this T u, string characterId) where T : UnlockableData
        {
            u.hasCharacterUnlock = true;
            u.character = characterId;

            return u;
        }

        public static T AddToDatabase<T>(this T u) where T : UnlockableData
        {
            var unlocksDB = infoHolder.UnlockableManager._unlockableDB;
            unlocksDB._miscUnlockableData = unlocksDB._miscUnlockableData.AddToArray(u);

            return u;
        }
    }

    public class CustomUnlockableData : UnlockableData
    {
        public bool hasCustomAchievementUnlock;
        public string customAchievementID;
    }
}
