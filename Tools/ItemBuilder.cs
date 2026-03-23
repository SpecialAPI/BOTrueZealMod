using Steamworks;
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

        public static T NewItem<T>(string id) where T : BaseWearableSO
        {
            var w = CreateScriptable<T>();
            w.name = GetID(id);
            w.staticModifiers = [];

            return w;
        }

        public static T SetName<T>(this T w, string name) where T : BaseWearableSO
        {
            w._itemName = name;

            return w;
        }

        public static T SetFlavor<T>(this T w, string flavor) where T : BaseWearableSO
        {
            w._flavourText = flavor;

            return w;
        }

        public static T SetDescription<T>(this T w, string description) where T : BaseWearableSO
        {
            w._description = description;

            return w;
        }

        public static T SetSprite<T>(this T w, string spriteName) where T : BaseWearableSO
        {
            w.wearableImage = LoadSprite(spriteName);

            return w;
        }

        public static T SetSprite<T>(this T w, Sprite sprite) where T : BaseWearableSO
        {
            w.wearableImage = sprite;

            return w;
        }

        public static T SetBasicInformation<T>(this T w, string name, string flavor, string description, string spriteName) where T : BaseWearableSO
        {
            w._itemName = name;
            w._flavourText = flavor;
            w._description = description;
            w.wearableImage = LoadSprite(spriteName);

            return w;
        }

        public static T SetBasicInformation<T>(this T w, string name, string flavor, string description, Sprite sprite) where T : BaseWearableSO
        {
            w._itemName = name;
            w._flavourText = flavor;
            w._description = description;
            w.wearableImage = sprite;

            return w;
        }

        public static T SetPrice<T>(this T w, int price) where T : BaseWearableSO
        {
            w.shopPrice = price;

            return w;
        }

        public static T SetStaticModifiers<T>(this T w, params WearableStaticModifierSetterSO[] modifiers) where T : BaseWearableSO
        {
            w.staticModifiers = modifiers ?? [];

            return w;
        }

        public static T AddItemTypes<T>(this T w, params ItemType[] itemTypes) where T : BaseWearableSO
        {
            w._itemTypes = [.. w._itemTypes ?? [], .. itemTypes];

            return w;
        }

        public static T SetStartsLocked<T>(this T w, bool startsLocked) where T : BaseWearableSO
        {
            if (w.startsLocked == startsLocked)
                return w;

            w.startsLocked = startsLocked;
            return w;
        }

        public static T AddToTreasure<T>(this T w) where T : BaseWearableSO
        {
            w.AddWithoutItemPools();
            if (itemPool != null)
                itemPool._TreasurePool = itemPool._TreasurePool.AddToArray(w.name);
            else
                treasuresToAdd.Add(w.name);

            return w;
        }

        public static T AddToShop<T>(this T w) where T : BaseWearableSO
        {
            w.AddWithoutItemPools();
            w.isShopItem = true;
            
            if (itemPool != null)
                itemPool._ShopPool = itemPool._ShopPool.AddToArray(w.name);
            else
                shopItemsToAdd.Add(w.name);

            return w;
        }

        public static T AddToFishPool<T>(this T w, int fishingRodWeight, int canOfWormsWelsCatfishWeight) where T : BaseWearableSO
        {
            if (fishingRodWeight > 0)
            {
                var fishingRodW = LoadedAssetsHandler.GetWearable("FishingRod_TW");
                if(fishingRodW != null && fishingRodW is PerformEffectWearable fishingRodPEW && fishingRodPEW.effects.FindEffectSO<ExtraLootListEffect>() is ExtraLootListEffect fishingRodLootList)
                {
                    var probability = new LootItemProbability()
                    {
                        itemName = w.name,
                        probability = fishingRodWeight
                    };

                    if (w.startsLocked)
                        fishingRodLootList._lockedLootableItems = [..fishingRodLootList._lockedLootableItems ?? [], probability];
                    else
                        fishingRodLootList._lootableItems = [..fishingRodLootList._lootableItems ?? [], probability];
                }
            }
            if (canOfWormsWelsCatfishWeight > 0)
            {
                var canOfWormsW = LoadedAssetsHandler.GetWearable("CanOfWorms_SW");
                if (canOfWormsW != null && canOfWormsW is PerformEffectWearable canOfWormsPEW && canOfWormsPEW.effects.FindEffectSO<ExtraLootListEffect>() is ExtraLootListEffect fishingRodLootList)
                {
                    var probability = new LootItemProbability()
                    {
                        itemName = w.name,
                        probability = canOfWormsWelsCatfishWeight
                    };

                    if (w.startsLocked)
                        fishingRodLootList._lockedLootableItems = [.. fishingRodLootList._lockedLootableItems ?? [], probability];
                    else
                        fishingRodLootList._lootableItems = [.. fishingRodLootList._lootableItems ?? [], probability];
                }
            }

            return w;
        }

        public static T AddWithoutItemPools<T>(this T w) where T : BaseWearableSO
        {
            // Check if the item is already added to avoid causing errors
            if (!LoadedAssetsHandler.LoadedWearables.ContainsKey(w.name))
                LoadedAssetsHandler.LoadedWearables.Add(w.name, w);

            return w;
        }

        public static ExtraAbility_Wearable_SMS ExtraAbilityModifier(CharacterAbility ab)
        {
            var mod = CreateScriptable<ExtraAbility_Wearable_SMS>();
            mod._extraAbility = ab;

            return mod;
        }

        public static BasicAbilityChange_Wearable_SMS BasicAbilityModifier(CharacterAbility ab)
        {
            var mod = CreateScriptable<BasicAbilityChange_Wearable_SMS>();
            mod._basicAbility = ab;

            return mod;
        }

        public static RankChange_Wearable_SMS RankChangeModifier(int rankAddition)
        {
            var mod = CreateScriptable<RankChange_Wearable_SMS>();
            mod._rankAdditive = rankAddition;

            return mod;
        }

        public static MaxHealthChange_Wearable_SMS MaxHealthModifier(int maxHealthChange, bool changeIsPercentage = false)
        {
            var mod = CreateScriptable<MaxHealthChange_Wearable_SMS>();
            mod.maxHealthChange = maxHealthChange;
            mod.isChangePercentage = changeIsPercentage;

            return mod;
        }

        public static HealthColorChange_Wearable_SMS HealthColorModifier(ManaColorSO healthColor)
        {
            var mod = CreateScriptable<HealthColorChange_Wearable_SMS>();
            mod._healthColor = healthColor;

            return mod;
        }

        public static ExtraPassiveAbility_Wearable_SMS ExtraPassiveModifier(BasePassiveAbilitySO passive)
        {
            var mod = CreateScriptable<ExtraPassiveAbility_Wearable_SMS>();
            mod._extraPassiveAbility = passive;

            return mod;
        }

        public static AbilitiesUsageChange_Wearable_SMS UsedAbilitiesChangeModifier(bool usesBasicAbility, bool usesAllAbilities)
        {
            var mod = CreateScriptable<AbilitiesUsageChange_Wearable_SMS>();
            mod._usesBasicAbility = usesBasicAbility;
            mod._usesAllAbilities = usesAllAbilities;

            return mod;
        }

        public static CurrencyMultiplierChange_Wearable_SMS CurrencyMultiplierModifier(int currencyMultAddition)
        {
            var mod = CreateScriptable<CurrencyMultiplierChange_Wearable_SMS>();
            mod._currencyMultiplier = currencyMultAddition;

            return mod;
        }
    }
}
