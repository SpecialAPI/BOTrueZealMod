using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Tools
{
    public static class AbilityBuilder
    {
        private static readonly Dictionary<Type, Dictionary<string, AbilitySO>> abilityReferences = [];

        public static T AbilityReference<T>(string id) where T : AbilitySO
        {
            if (!abilityReferences.TryGetValue(typeof(T), out var abReferencesForType))
                abilityReferences[typeof(T)] = abReferencesForType = [];

            if (!abReferencesForType.TryGetValue(id, out var ab))
            {
                var newAb = CreateScriptable<T>();
                abReferencesForType[id] = newAb;
                return newAb;
            }

            return (T)ab;
        }

        public static AbilitySO AbilityReference(string id)
        {
            return AbilityReference<AbilitySO>(id);
        }

        public static AbilitySO NewAbility(string id_A)
        {
            return NewAbility<AbilitySO>(id_A);
        }

        public static T NewAbility<T>(string id_A) where T : AbilitySO
        {
            var ab = GetOrCreateNewAbilityReference<T>(id_A);
            ab.name = GetID(id_A);
            ab.effects = [];
            ab.intents = [];

            ab.priority = Priority.Normal;

            return ab;
        }

        private static T GetOrCreateNewAbilityReference<T>(string id) where T : AbilitySO
        {
            if (!abilityReferences.TryGetValue(typeof(T), out var abReferencesForType))
                abilityReferences[typeof(T)] = abReferencesForType = [];

            var exists = abReferencesForType.TryGetValue(id, out var ab);
            if (exists && !string.IsNullOrEmpty(ab.name))
            {
                exists = false;
            }

            if (!exists)
            {
                var newAb = CreateScriptable<T>();
                abReferencesForType[id] = newAb;
                return newAb;
            }

            return (T)ab;
        }

        public static T SetName<T>(this T ab, string name) where T : AbilitySO
        {
            ab._abilityName = name;

            return ab;
        }

        public static T SetDescription<T>(this T ab, string description) where T : AbilitySO
        {
            ab._description = description;

            return ab;
        }

        public static T SetSprite<T>(this T ab, string spriteName) where T : AbilitySO
        {
            ab.abilitySprite = LoadSprite(spriteName);

            return ab;
        }

        public static T SetSprite<T>(this T ab, Sprite sprite) where T : AbilitySO
        {
            ab.abilitySprite = sprite;

            return ab;
        }

        public static T SetBasicInformation<T>(this T ab, string name, string description, string spriteName = null) where T : AbilitySO
        {
            ab._abilityName = name;
            ab._description = description;

            if (spriteName != null)
            {
                ab.abilitySprite = LoadSprite(spriteName);
            }

            return ab;
        }

        public static T SetBasicInformation<T>(this T ab, string name, string description, Sprite sprite) where T : AbilitySO
        {
            ab._abilityName = name;
            ab._description = description;
            ab.abilitySprite = sprite;

            return ab;
        }

        public static T SetBasicInformationEnemy<T>(this T ab, string name, string description, bool addToDatabase = true, bool addToPool = true) where T : AbilitySO
        {
            ab._abilityName = name;
            ab._description = description;
            ab.abilitySprite = LoadedAssetsHandler.GetEnemyAbility("Crush_A").abilitySprite;

            if (addToDatabase)
                ab.AddToEnemyDatabase(addToPool);

            return ab;
        }

        public static T SetBasicInformationCharacter<T>(this T ab, string name, string description, string spriteName = null, bool addToDatabase = true, bool addToPool = true) where T : AbilitySO
        {
            ab._abilityName = name;
            ab._description = description;

            if (spriteName != null)
            {
                ab.abilitySprite = LoadSprite(spriteName);
            }

            if (addToDatabase)
                ab.AddToCharacterDatabase(addToPool);

            return ab;
        }

        public static T SetBasicInformationCharacter<T>(this T ab, string name, string description, Sprite sprite, bool addToDatabase = true, bool addToPool = true) where T : AbilitySO
        {
            ab._abilityName = name;
            ab._description = description;
            ab.abilitySprite = sprite;

            if (addToDatabase)
                ab.AddToCharacterDatabase(addToPool);

            return ab;
        }

        public static T SetEffects<T>(this T ab, List<EffectInfo> effects) where T : AbilitySO
        {
            ab.effects = [.. effects];

            return ab;
        }

        public static T SetIntents<T>(this T ab, List<IntentTargetInfo> intents) where T : AbilitySO
        {
            ab.intents = [.. intents];

            return ab;
        }

        public static T AddIntent<T>(this T ab, BaseCombatTargettingSO target, params IntentType[] intents) where T : AbilitySO
        {
            return ab.AddIntent(TargetIntent(target, intents));
        }

        public static T AddIntent<T>(this T ab, IntentTargetInfo intent) where T : AbilitySO
        {
            ab.intents = ab.intents.AddToArray(intent);

            return ab;
        }

        public static T SetVisuals<T>(this T ab, AttackVisualsSO visuals, BaseCombatTargettingSO animationTarget) where T : AbilitySO
        {
            ab.visuals = visuals;
            ab.animationTarget = animationTarget;

            return ab;
        }

        public static T SetStoredValue<T>(this T ab, UnitStoredValueNames storedValue) where T : AbilitySO
        {
            ab.specialStoredValue = storedValue;

            return ab;
        }

        public static T SetPriority<T>(this T ab, PrioritySO priority) where T : AbilitySO
        {
            ab.priority = priority;

            return ab;
        }

        public static T AddToCharacterDatabase<T>(this T ab, bool addToPool = true) where T : AbilitySO
        {
            LoadedAssetsHandler.LoadedCharacterAbilities[ab.name] = ab;

            if (addToPool)
                LoadedAssetsHandler.GetAbilityDB()._characterAbilityPool.Add(ab.name);

            return ab;
        }

        public static T AddToEnemyDatabase<T>(this T ab, bool addToPool = true) where T : AbilitySO
        {
            LoadedAssetsHandler.LoadedEnemyAbilities[ab.name] = ab;

            if (addToPool)
                LoadedAssetsHandler.GetAbilityDB()._enemyAbilityPool.Add(ab.name);

            return ab;
        }

        public static EnemyAbilityInfo EnemyAbility<T>(this T ab, RaritySO rarity, PrioritySO priority = null) where T : AbilitySO
        {
            if (priority != null)
                ab.priority = priority;

            return new()
            {
                ability = ab,
                rarity = rarity,
            };
        }

        public static CharacterAbility CharacterAbility<T>(this T ab, params ManaColorSO[] cost) where T : AbilitySO
        {
            return new()
            {
                ability = ab,
                cost = cost
            };
        }

        public static ExtraAbilityInfo ExtraAbility<T>(this T ab, RaritySO rarity, PrioritySO priority = null) where T : AbilitySO
        {
            if (priority != null)
                ab.priority = priority;

            return new()
            {
                ability = ab,
                rarity = rarity
            };
        }

        public static ExtraAbilityInfo ExtraAbility<T>(this T ab, params ManaColorSO[] cost) where T : AbilitySO
        {
            return new()
            {
                ability = ab,
                cost = cost
            };
        }

        public static ExtraAbilityInfo ExtraAbility<T>(this T ab, RaritySO rarity, params ManaColorSO[] cost) where T : AbilitySO
        {
            return new()
            {
                ability = ab,
                rarity = rarity,
                cost = cost
            };
        }

        public static ExtraAbilityInfo ExtraAbility<T>(this T ab, RaritySO rarity, PrioritySO priority, params ManaColorSO[] cost) where T : AbilitySO
        {
            if (priority != null)
                ab.priority = priority;

            return new()
            {
                ability = ab,
                rarity = rarity,
                cost = cost
            };
        }
    }
}
