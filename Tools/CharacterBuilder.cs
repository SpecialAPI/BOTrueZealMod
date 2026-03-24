using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.TextCore;

namespace BOTrueZealMod.Tools
{
    [HarmonyPatch]
    public static class CharacterBuilder
    {
        public static SelectableCharactersSO selectableCharacters;
        private static readonly List<SelectableCharacterData> selectableCharsToAdd = [];
        private static readonly Dictionary<CharacterRefString, CharacterIgnoredAbilities> dpsToAdd = [];
        private static readonly Dictionary<CharacterRefString, CharacterIgnoredAbilities> supportsToAdd = [];
        private static readonly HashSet<string> charactersUnlockedByDefault = [];

        private static int _rank = -1;

        [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.Start))]
        [HarmonyPrefix]
        private static void AddCharactersToMenu(MainMenuController __instance)
        {
            if (selectableCharacters != null)
                return;

            if (__instance._charSelectionDB == null)
                return;

            selectableCharacters = __instance._charSelectionDB;

            selectableCharacters._characters = selectableCharacters._characters.AddRangeToArray([..selectableCharsToAdd]);
            foreach(var kvp in dpsToAdd)
                selectableCharacters._dpsCharacters.AddOrSet(kvp.Key, kvp.Value);
            foreach(var kvp in supportsToAdd)
                selectableCharacters._supportCharacters.AddOrSet(kvp.Key, kvp.Value);

            selectableCharsToAdd.Clear();
            dpsToAdd.Clear();
            supportsToAdd.Clear();
        }

        [HarmonyPatch(typeof(InGameDataSO), nameof(InGameDataSO.GetUnlockedCharacters))]
        [HarmonyPostfix]
        private static void AddUnlockedByDefaultCharacters1(ref string[] __result)
        {
            var toAdd = new HashSet<string>();

            foreach(var ch in charactersUnlockedByDefault)
            {
                if (Array.IndexOf(__result, ch) >= 0)
                    continue;

                toAdd.Add(ch);
            }

            __result = __result.Concat(toAdd).ToArray();
        }

        [HarmonyPatch(typeof(SelectableCharacterData), nameof(SelectableCharacterData.TryLoadIfAvailable))]
        [HarmonyPostfix]
        private static void AddUnlockedByDefaultCharacters1(SelectableCharacterData __instance)
        {
            if (__instance.LoadedCharacter != null)
                return;

            if (charactersUnlockedByDefault.Contains(__instance._characterName))
                __instance.LoadedCharacter = LoadedAssetsHandler.GetCharcater(__instance._characterName);
        }

        public static CharacterSO NewCharacter(string id_CH, EntityIDs entityId)
        {
            return NewCharacter<CharacterSO>(id_CH, entityId);
        }

        public static T NewCharacter<T>(string id_CH, EntityIDs entityId) where T : CharacterSO
        {
            var ch = CreateScriptable<T>();
            ch.name = GetID(id_CH);
            ch.characterEntityID = entityId;

            ch.healthColor = Pigments.Purple;
            ch.passiveAbilities = [];
            ch.basicCharAbility = new()
            {
                ability = LoadedAssetsHandler.GetCharacterAbility("Slap_A"),
                cost = [Pigments.Yellow]
            };

            ch.damageSound = string.Empty;
            ch.deathSound = string.Empty;
            ch.dxSound = string.Empty;

            //ch.m_StartsLocked = false;

            return ch;
        }

        public static T SetBasicInformation<T>(this T ch, string name, ManaColorSO healthColor, string frontSpriteName, string backSpriteName, string overworldSpriteName) where T : CharacterSO
        {
            ch._characterName = name;
            ch.healthColor = healthColor;
            ch.characterSprite = LoadSprite(frontSpriteName);
            ch.characterBackSprite = LoadSprite(backSpriteName);
            ch.characterOWSprite = LoadSprite(overworldSpriteName, new(0.5f, 0f));

            return ch;
        }

        public static T SetBasicInformation<T>(this T ch, string name, ManaColorSO healthColor, Sprite frontSprite, Sprite backSprite, Sprite overworldSprite) where T : CharacterSO
        {
            ch._characterName = name;
            ch.healthColor = healthColor;
            ch.characterSprite = frontSprite;
            ch.characterBackSprite = backSprite;
            ch.characterOWSprite = overworldSprite;

            return ch;
        }

        public static T SetName<T>(this T ch, string name) where T : CharacterSO
        {
            ch._characterName = name;

            return ch;
        }

        public static T SetHealthColor<T>(this T ch, ManaColorSO color) where T : CharacterSO
        {
            ch.healthColor = color;

            return ch;
        }

        public static T SetSprites<T>(this T ch, string frontSpriteName, string backSpriteName, string overworldSpriteName) where T : CharacterSO
        {
            ch.characterSprite = LoadSprite(frontSpriteName);
            ch.characterBackSprite = LoadSprite(backSpriteName);
            ch.characterOWSprite = LoadSprite(overworldSpriteName, new(0.5f, 0f));

            return ch;
        }

        public static T SetSprites<T>(this T ch, Sprite frontSprite, Sprite backSprite, Sprite overworldSprite) where T : CharacterSO
        {
            ch.characterSprite = frontSprite;
            ch.characterBackSprite = backSprite;
            ch.characterOWSprite = overworldSprite;

            return ch;
        }

        public static T SetSounds<T>(this T ch, string damageSound, string deathSound, string dialogueSound) where T : CharacterSO
        {
            if (damageSound != null)
                ch.damageSound = damageSound;
            if (deathSound != null)
                ch.deathSound = deathSound;
            if (dialogueSound != null)
                ch.dxSound = dialogueSound;

            return ch;
        }

        public static T AddPassives<T>(this T ch, params BasePassiveAbilitySO[] passives) where T : CharacterSO
        {
            ch.passiveAbilities = ch.passiveAbilities.AddRangeToArray(passives);

            return ch;
        }

        public static T AddRankedData<T>(this T ch, params CharacterRankedData[] rankedData) where T : CharacterSO
        {
            ch.rankedData = ch.rankedData.AddRangeToArray(rankedData);

            return ch;
        }

        public static T SetMovesOnOverworld<T>(this T ch, bool moves) where T : CharacterSO
        {
            ch.movesOnOverworld = moves;

            return ch;
        }

        public static T RankedDataSetup<T>(this T ch, int ranks, Func<int, int, CharacterRankedData> rankSetup) where T : CharacterSO
        {
            ch.rankedData = new CharacterRankedData[ranks];

            for (var i = 0; i < ranks; i++)
            {
                _rank = i;
                ch.rankedData[i] = rankSetup(i, i + 1);

                _rank = -1;
            }

            return ch;
        }

        public static T RankedValue<T>(params T[] rankValues)
        {
            return rankValues[Mathf.Clamp(_rank, 0, rankValues.Length - 1)];
        }

        public static T RankedValueManual<T>(int rank, params T[] rankValues)
        {
            return rankValues[Mathf.Clamp(rank, 0, rankValues.Length - 1)];
        }

        public static T SetBasicAbility<T>(this T ch, CharacterAbility basicAbility) where T : CharacterSO
        {
            ch.basicCharAbility = basicAbility;

            return ch;
        }

        public static T SetAbilityUsage<T>(this T ch, bool usesAllAbilities, bool usesBasicAbility) where T : CharacterSO
        {
            ch.usesAllAbilities = usesAllAbilities;
            ch.usesBasicAbility = usesBasicAbility;

            return ch;
        }

        public static T SetUnitType<T>(this T ch, UnitType unitType) where T : CharacterSO
        {
            ch.unitType = unitType;

            return ch;
        }

        public static T AddFinalBossUnlock<T>(this T ch, BossType bossId, UnlockableData unlock) where T : CharacterSO
        {
            if (unlock == null)
                return ch;

            //if (unlock.hasModdedAchievementUnlock)
            //    ch.m_BossAchData.Add(new(bossId, unlock.moddedAchievementID));

            var unlockConnection = new EntityUnlockConnection()
            {
                entityID = ch.characterEntityID,
                unlockID = unlock.id
            };

            var unlocksDB = infoHolder.UnlockableManager._unlockableDB;
            if (bossId == BossType.OsmanSinnoks)
                unlocksDB._osmanConnections = unlocksDB._osmanConnections.AddToArray(unlockConnection);
            else if (bossId == BossType.Heaven)
                unlocksDB._heavenConnections = unlocksDB._heavenConnections.AddToArray(unlockConnection);
            else
                Debug.LogError($"Boss {bossId} is unsupported...");

            unlocksDB._bossDefeatUnlockableData = unlocksDB._bossDefeatUnlockableData.AddToArray(unlock);

            return ch;
        }

        public static T AddToDatabase<T>(this T ch, bool appearsInShops = true, bool locked = false) where T : CharacterSO
        {
            LoadedAssetsHandler.LoadedCharacters[ch.name] = ch;

            if (!appearsInShops)
            {
                // todo
                var areas = new List<string>()
                {

                };

                foreach(var a in areas)
                {
                    if (LoadedAssetsHandler.GetZoneDB(a) is not ZoneBGDataBaseSO bg)
                        continue;

                    bg._omittedCharacters.Add(ch.name);
                }
            }

            if(locked && charactersUnlockedByDefault.Contains(ch.name))
                charactersUnlockedByDefault.Remove(ch.name);
            else if(!locked && !charactersUnlockedByDefault.Contains(ch.name))
                charactersUnlockedByDefault.Add(ch.name);

            return ch;
        }

        public static SelectableCharacterData GenerateMenuCharacter<T>(this T ch, string unlockedSpriteName, string lockedSpriteName = null) where T : CharacterSO
        {
            var unlockedSprite = LoadSprite(unlockedSpriteName);
            var lockedSprite = string.IsNullOrEmpty(lockedSpriteName) ? null : LoadSprite(lockedSpriteName);

            return new(ch.name, unlockedSprite, lockedSprite != null ? lockedSprite : unlockedSprite);
        }

        public static SelectableCharacterData GenerateMenuCharacter<T>(this T ch, Sprite unlockedSprite, Sprite lockedSprite = null) where T : CharacterSO
        {
            return new(ch.name, unlockedSprite, lockedSprite != null ? lockedSprite : unlockedSprite);
        }

        public static T AddToDatabase<T>(this T selCh) where T : SelectableCharacterData
        {
            if (selectableCharacters != null)
                selectableCharacters._characters = selectableCharacters._characters.AddToArray(selCh);
            else
                selectableCharsToAdd.Add(selCh);

            return selCh;
        }

        public static T SetAsFullDPS<T>(this T selCh) where T : SelectableCharacterData
        {
            var refStr = new CharacterRefString(selCh.CharacterName);
            var ignoredAbs = new CharacterIgnoredAbilities() { ignoredAbilities = [] };

            if (selectableCharacters != null)
                selectableCharacters._dpsCharacters.AddOrSet(refStr, ignoredAbs);
            else
                dpsToAdd[refStr] = ignoredAbs;

            return selCh;
        }

        public static T AddDPSSets<T>(this T selCh, params int[] sets) where T : SelectableCharacterData
        {
            var refStr = new CharacterRefString(selCh.CharacterName);

            if (selectableCharacters != null)
            {
                if (selectableCharacters._dpsCharacters.TryGetValue(refStr, out var dpsSets))
                {
                    if (dpsSets.ignoredAbilities != null && dpsSets.ignoredAbilities.Count > 0)
                        dpsSets.ignoredAbilities.AddRange(sets);
                }
                else
                    selectableCharacters._dpsCharacters.Add(refStr, new() { ignoredAbilities = [..sets] });
            }
            else
            {
                if (dpsToAdd.TryGetValue(refStr, out var dpsSets))
                {
                    if (dpsSets.ignoredAbilities != null && dpsSets.ignoredAbilities.Count > 0)
                        dpsSets.ignoredAbilities.AddRange(sets);
                }
                else
                    dpsToAdd.Add(refStr, new() { ignoredAbilities = [..sets] });
            }

            return selCh;
        }

        public static T SetAsFullSupport<T>(this T selCh) where T : SelectableCharacterData
        {
            var refStr = new CharacterRefString(selCh.CharacterName);
            var ignoredAbs = new CharacterIgnoredAbilities() { ignoredAbilities = [] };

            if (selectableCharacters != null)
                selectableCharacters._supportCharacters.AddOrSet(refStr, ignoredAbs);
            else
                supportsToAdd[refStr] = ignoredAbs;

            return selCh;
        }

        public static T AddSupportSets<T>(this T selCh, params int[] sets) where T : SelectableCharacterData
        {
            var refStr = new CharacterRefString(selCh.CharacterName);

            if (selectableCharacters != null)
            {
                if (selectableCharacters._supportCharacters.TryGetValue(refStr, out var supportSets))
                {
                    if (supportSets.ignoredAbilities != null && supportSets.ignoredAbilities.Count > 0)
                        supportSets.ignoredAbilities.AddRange(sets);
                }
                else
                    selectableCharacters._supportCharacters.Add(refStr, new() { ignoredAbilities = [.. sets] });
            }
            else
            {
                if (supportsToAdd.TryGetValue(refStr, out var supportSets))
                {
                    if (supportSets.ignoredAbilities != null && supportSets.ignoredAbilities.Count > 0)
                        supportSets.ignoredAbilities.AddRange(sets);
                }
                else
                    supportsToAdd.Add(refStr, new() { ignoredAbilities = [.. sets] });
            }

            return selCh;
        }
    }
}
