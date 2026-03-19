using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.TextCore;

namespace BOTrueZealMod.Tools
{
    public static class CharacterBuilder
    {
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

            return ch;
        }

        // todo
        //public static SelectableCharacterData GenerateMenuCharacter<T>(this T ch, string unlockedSpriteName, string lockedSpriteName = null) where T : CharacterSO
        //{
        //    profile ??= ProfileManager.GetProfile(Assembly.GetCallingAssembly());
        //    if (!ProfileManager.EnsureProfileExists(profile))
        //        return null;

        //    var unlockedSprite = LoadSprite(unlockedSpriteName);
        //    var lockedSprite = string.IsNullOrEmpty(lockedSpriteName) ? null : LoadSprite(lockedSpriteName);

        //    return new(ch.name, unlockedSprite, lockedSprite != null ? lockedSprite : unlockedSprite);
        //}

        //public static SelectableCharacterData GenerateMenuCharacter<T>(this T ch, Sprite unlockedSprite, Sprite lockedSprite = null) where T : CharacterSO
        //{
        //    return new(ch.name, unlockedSprite, lockedSprite != null ? lockedSprite : unlockedSprite);
        //}

        //public static T AddToDatabase<T>(this T selCh) where T : SelectableCharacterData
        //{
        //    CharacterDB.SelectableCharacters.Add(selCh);

        //    return selCh;
        //}

        //public static T SetAsFullDPS<T>(this T selCh) where T : SelectableCharacterData
        //{
        //    CharacterDB._dpsCharacters.Add(new(selCh.CharacterName), new([]));

        //    return selCh;
        //}

        //public static T SetAsFullSupport<T>(this T selCh) where T : SelectableCharacterData
        //{
        //    CharacterDB._supportCharacters.Add(new(selCh.CharacterName), new([]));

        //    return selCh;
        //}

        private static int _rank = -1;
    }
}
