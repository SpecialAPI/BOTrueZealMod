using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Tools;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.TextCore;

namespace BOTrueZealMod.Tools
{
    [HarmonyPatch]
    public static class AchievementBuilder
    {
        public static readonly Dictionary<string, CustomAchievement> achievements = [];
        public static readonly Dictionary<AchievementUnlockType, HashSet<CustomAchievement>> achievementsByCategory = [];
        private static bool achievementsInitialized;
        private static readonly string achievementCompleteDataKeyFormat = GetID("ACHCompleted_{0}");

        [HarmonyPatch(typeof(AchievementsManagerData), nameof(AchievementsManagerData.AchievementInitialization))]
        [HarmonyPostfix]
        private static void InitCustomAchievements()
        {
            if (achievementsInitialized)
                return;

            var gameData = infoHolder.Game;
            if(gameData == null)
            {
                Debug.LogError($"Game data is null... {new StackTrace()}");
                return;
            }

            foreach(var ach in achievements.Values)
                ach.unlocked = gameData.IsCustomAchCompleted(ach.id);

            achievementsInitialized = true;
        }

        [HarmonyPatch(typeof(UnlockedAchievementsUIHandler), nameof(UnlockedAchievementsUIHandler.PopulateList))]
        [HarmonyPostfix]
        private static void AddAchievementsToMenu(UnlockedAchievementsUIHandler __instance, UnlockListUIPanel list, AchievementUnlockType listType, Sprite lockSprite)
        {
            const float SizeDeltaYIncreasePerRow = 150f;

            if (!achievementsByCategory.TryGetValue(listType, out var achs) || achs.Count == 0)
                return;

            if (list._icons.Length == 0)
                return;

            var firstValidIconIdx = 0;
            for(; firstValidIconIdx < list._icons.Length; firstValidIconIdx++)
            {
                var icon = list._icons[firstValidIconIdx];

                if (icon != null && !icon._holder.activeSelf)
                    break;
            }

            var firstIcon = list._icons[0];
            var firstRow = firstIcon.transform.parent.gameObject;
            var iconZone = firstRow.transform.parent.gameObject;

            var newRows = 0;

            foreach(var (i, ach) in achs.Enumerate())
            {
                var iconIdx = firstValidIconIdx + i;
                if(iconIdx >= list._icons.Length)
                {
                    var newRow = Object.Instantiate(firstRow, iconZone.transform);
                    var newIcons = new List<UnlockIconUILayout>();
                    foreach(Transform iconTransform in newRow.transform)
                    {
                        var iconComp = iconTransform.GetComponent<UnlockIconUILayout>();
                        if(iconComp == null)
                            continue;

                        newIcons.Add(iconComp);

                        iconComp._holder.SetActive(false);
                        iconComp._calls = null;
                        iconComp._listID = -1;
                        iconComp._id = -1;
                    }

                    list._icons = [..list._icons, ..newIcons];
                    newRows++;
                }

                if (iconIdx >= list._icons.Length)
                {
                    Debug.LogError($"New achievements icons weren't successfully added... {new StackTrace()}");
                    break;
                }

                var icon = list._icons[iconIdx];

                var evilInfoHolder = icon.GetOrAddComponent<EvilAchievementInfoHolder>();
                evilInfoHolder.customAchID = ach.id;
                evilInfoHolder.achievementUI = __instance;

                var sprite = ach.unlocked ? ach.unlockedSprite : lockSprite;
                icon.SetInformation(evilInfoHolder, -1, -1, sprite);
            }

            if (newRows > 0)
            {
                if (iconZone.transform is RectTransform iconZoneRT)
                    LayoutRebuilder.ForceRebuildLayoutImmediate(iconZoneRT);

                if (list.transform is RectTransform listRT)
                {
                    listRT.sizeDelta += new Vector2(0f, newRows * SizeDeltaYIncreasePerRow);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(listRT);
                }
            }
        }

        private class EvilAchievementInfoHolder : MonoBehaviour, IUnlockCalls
        {
            public string customAchID;
            public UnlockedAchievementsUIHandler achievementUI;

            public void OnIconEnter(int listID, int id) => achievementUI.ShowCustomAchievement(customAchID);
            public void OnIconExit() => achievementUI.OnIconExit();
        }

        private static void ShowCustomAchievement(this UnlockedAchievementsUIHandler achUI, string id)
        {
            if (achUI == null)
                return;

            if (!achievements.TryGetValue(id, out var ach))
                return;

            var sprite = ach.unlocked ? ach.unlockedSprite : achUI._unlockInfoDB.LockedAchSprite;
            achUI._extraPanel.SetCustomAchievementInformation(ach, sprite);
            achUI._achExtraPanel.TryOpenAchievementExtraMenu(AchievementExtraDataInfoType.None);
        }

        private static void SetCustomAchievementInformation(this ExtraInformationUIHandler extraUI, CustomAchievement ach, Sprite sprite)
        {
            extraUI._willBeClosing = false;

            var loc = ach.GetAchLocData();
            var name = loc.text;
            var description = (!ach.unlocked && ach.isSecret) ? loc.subDescription : loc.description;
            extraUI._achievementLayout.SetInformation(sprite, name, description);

            extraUI._attackLayout.HideInformation();
            extraUI._itemLayout.HideInformation();
            extraUI._passiveLayout.HideInformation();
            extraUI._glossaryLayout.HideInformation();
            extraUI.OpenMenu();
        }

        [HarmonyPatch(typeof(SelectableCharacterData), nameof(SelectableCharacterData.SetAchievementState))]
        [HarmonyPostfix]
        private static void ProcessCustomHeavenAndOsmanAchievements(SelectableCharacterData __instance)
        {
            if (__instance is not CustomSelectableCharacterData customSelCh)
                return;

            if (customSelCh.hasCustomOsmanAchievement)
                __instance.HasTheWitness = IsCustomAchievementUnlocked(customSelCh.customOsmanAchievementID);

            if (customSelCh.hasCustomHeavenAchievement)
                __instance.HasTheDivine = IsCustomAchievementUnlocked(customSelCh.customHeavenAchievementID);
        }

        [HarmonyPatch(typeof(SelectableCharactersSO), nameof(SelectableCharactersSO.PrepareCharacters))]
        [HarmonyPostfix]
        private static void ProcessCustomHeavenAndOsmanAchievements2(SelectableCharactersSO __instance)
        {
            foreach(var ch in __instance._characters)
                ProcessCustomHeavenAndOsmanAchievements(ch);
        }

        [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.Start))]
        [HarmonyPostfix]
        private static void ProcessCustomHeavenAndOsmanAchievements3(MainMenuController __instance)
        {
            if (__instance._charSelectionDB == null)
                return;

            ProcessCustomHeavenAndOsmanAchievements2(__instance._charSelectionDB);
        }

        public static CustomAchievement NewAchievement(string ACH_achievementId, string name, string description)
        {
            var ach = new CustomAchievement(ACH_achievementId, name, description);
            return ach;
        }

        public static CustomAchievement SetSprite(this CustomAchievement ach, string unlockedSpriteName) 
        {
            ach.unlockedSprite = LoadSprite(unlockedSpriteName);

            return ach;
        }

        public static CustomAchievement SetSprite(this CustomAchievement ach, Sprite unlockedSprite) 
        {
            ach.unlockedSprite = unlockedSprite;

            return ach;
        }

        public static CustomAchievement AddToBaseCategory(this CustomAchievement ach, AchievementUnlockType category) 
        {
            achievements[ach.id] = ach;

            if (!achievementsByCategory.TryGetValue(category, out var achs))
                achievementsByCategory[category] = achs = [];
            achs.Add(ach);

            return ach;
        }

        private static string GetCustomAchCompletionDataKey(string achID) => string.Format(achievementCompleteDataKeyFormat, achID);

        private static bool IsCustomAchCompleted(this InGameDataSO gameData, string achID) => gameData.GetBoolData(GetCustomAchCompletionDataKey(achID));

        public static void UnlockCustomAchievement(this InGameDataSO gameData, string achID)
        {
            if (!achievements.TryGetValue(achID, out var ach))
                return;

            ach.unlocked = true;
            gameData.SetBoolData(GetCustomAchCompletionDataKey(ach.id), true);
        }

        public static bool IsCustomAchievementUnlocked(string achID)
        {
            if(!achievements.TryGetValue(achID, out var ach))
                return false;

            return ach.unlocked;
        }
    }

    public class CustomAchievement(string id, string name, string description)
    {
        public string id = id;
        public string name = name;
        public string description = description;
        public Sprite unlockedSprite;
        public bool isSecret;
        public string secretDescription;
        public AchievementExtraDataInfoType extraInfoType;

        public bool unlocked;

        private StringTrioData locItemData;
        private string locID = "";

        public StringTrioData GetAchLocData()
        {
            if (locID != LocUtils.LocDB.LocID || locItemData == null)
            {
                locID = LocUtils.LocDB.LocID;
                locItemData = LocUtils.LocDB.GetCustomAchievementData(id, name, description, secretDescription);
            }
            return locItemData;
        }
    }
}
