using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Tools
{
    public static class EnemyBuilder
    {
        private static readonly BaseAbilitySelectorSO RarityAbilitySelector = CreateScriptable<AbilitySelector_ByRarity>();

        public static EnemySO NewEnemy(string id_EN)
        {
            return NewEnemy<EnemySO>(id_EN);
        }

        public static T NewEnemy<T>(string id_EN) where T : EnemySO
        {
            var en = CreateScriptable<T>();

            en.name = GetID(id_EN);

            en.priority = Priority.Normal;
            en.abilitySelector = RarityAbilitySelector;

            en.abilities = [];
            en.passiveAbilities = [];
            en.enterEffects = [];
            en.exitEffects = [];
            en.enemyLoot = new() { _lootableItems = [] };

            en.health = 1;
            en.healthColor = Pigments.Red;
            en.size = 1;

            en.damageSound = string.Empty;
            en.deathSound = string.Empty;

            return en;
        }

        public static T SetName<T>(this T en, string name) where T : EnemySO
        {
            en._enemyName = name;

            return en;
        }

        public static T SetHealth<T>(this T en, int health, ManaColorSO healthColor) where T : EnemySO
        {
            en.health = health;
            en.healthColor = healthColor;

            return en;
        }

        public static T SetSpritesBoss<T>(this T en, string combatSpriteName, string overworldSpriteName, string corpseSpriteName) where T : EnemySO
        {
            en.enemySprite = LoadSprite(combatSpriteName);

            if (!string.IsNullOrEmpty(overworldSpriteName))
                en.enemyOverworldSprite = LoadSprite(overworldSpriteName, new(0.5f, 0f));
            else
                en.enemyOverworldSprite = en.enemySprite;

            if (!string.IsNullOrEmpty(corpseSpriteName))
                en.enemyOWCorpseSprite = LoadSprite(corpseSpriteName, new(0.5f, 0f));
            else
                en.enemyOWCorpseSprite = en.enemyOverworldSprite;

            return en;
        }

        public static T SetSprites<T>(this T en, string combatSpriteName, string overworldSpriteName, string corpseSpriteName) where T : EnemySO
        {
            en.enemySprite = LoadSprite(combatSpriteName, new(0.5f, 0f));

            if (!string.IsNullOrEmpty(overworldSpriteName))
                en.enemyOverworldSprite = LoadSprite(overworldSpriteName, new(0.5f, 0f));
            else
                en.enemyOverworldSprite = en.enemySprite;

            if (!string.IsNullOrEmpty(corpseSpriteName))
                en.enemyOWCorpseSprite = LoadSprite(corpseSpriteName, new(0.5f, 0f));
            else
                en.enemyOWCorpseSprite = en.enemyOverworldSprite;

            return en;
        }

        public static T SetSprites<T>(this T en, Sprite combatSprite, Sprite overworldSprite, Sprite corpseSprite) where T : EnemySO
        {
            en.enemySprite = combatSprite;

            if (overworldSprite != null)
                en.enemyOverworldSprite = overworldSprite;
            else
                en.enemyOverworldSprite = en.enemySprite;

            if (corpseSprite != null)
                en.enemyOWCorpseSprite = corpseSprite;
            else
                en.enemyOWCorpseSprite = en.enemyOverworldSprite;

            return en;
        }

        public static T SetSounds<T>(this T en, string damageSound, string deathSound) where T : EnemySO
        {
            if (damageSound != null)
                en.damageSound = damageSound;
            if (deathSound != null)
                en.deathSound = deathSound;

            return en;
        }

        public static T SetEnemyPrefab<T>(this T en, string prefabName, string gibsName = null) where T : EnemySO
        {
            var prefab = Bundle.LoadAsset<GameObject>(prefabName);
            var gibs = (ParticleSystem)null;

            if (!string.IsNullOrEmpty(gibsName))
            {
                var gibsObj = Bundle.LoadAsset<GameObject>(gibsName);
                gibs = gibsObj != null ? gibsObj.GetComponent<ParticleSystem>() : null;
            }

            return en.SetEnemyPrefab(prefab, gibs);
        }

        public static T SetEnemyPrefab<T>(this T en, GameObject prefab, ParticleSystem gibs = null) where T : EnemySO
        {
            if (prefab == null)
                return en;

            var layout = prefab.GetComponent<EnemyInFieldLayout>();
            if (layout == null)
                layout = prefab.AddComponent<EnemyInFieldLayout>();

            if (gibs != null)
                layout._gibs = gibs;

            en.enemyTemplate = layout;

            return en;
        }

        public static T AddPassives<T>(this T en, params BasePassiveAbilitySO[] passives) where T : EnemySO
        {
            en.passiveAbilities = en.passiveAbilities.AddRangeToArray(passives);

            return en;
        }

        public static T SetAbilities<T>(this T en, List<EnemyAbilityInfo> abilities) where T : EnemySO
        {
            en.abilities = [..abilities];

            return en;
        }

        public static T SetUnitType<T>(this T en, UnitType unitType) where T : EnemySO
        {
            en.unitType = unitType;

            return en;
        }

        public static T AddToDatabase<T>(this T en, bool addToBronzoPool = false, bool addToSepulchrePool = false, bool addToSmallPool = false) where T : EnemySO
        {
            LoadedAssetsHandler.LoadedEnemies[en.name] = en;

            if (addToBronzoPool)
                en.AddToBronzoPool();
            if (addToSepulchrePool)
                en.AddToSepulchrePool();
            if (addToSmallPool)
                en.AddToSmallPool();

            return en;
        }

        public static T AddToBronzoPool<T>(this T en) where T : EnemySO
        {
            var effect = GetAnyAbility("Unity_A").effects.FindEffectSO<SpawnRandomEnemyAnywhereEffect>();
            effect._enemies = effect._enemies.AddToArray(en);

            return en;
        }

        public static T AddToSepulchrePool<T>(this T en) where T : EnemySO
        {
            var effect = GetAnyAbility("Repent_A").effects.FindEffectSO<SpawnMassivelyEverywhereUsingHealthEffect>();
            effect._possibleEnemies = effect._possibleEnemies.AddToArray(en);

            return en;
        }

        public static T AddToSmallPool<T>(this T en) where T : EnemySO
        {
            var effect = GetAnyAbility("Regurgitate_A").effects.FindEffectSO<SpawnRandomEnemyAnywhereEffect>();
            effect._enemies = effect._enemies.AddToArray(en);

            return en;
        }
    }
}
