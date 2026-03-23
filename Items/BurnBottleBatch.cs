using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Items
{
    public static class BurnBottleBatch
    {
        public static void Init()
        {
            var name = "Burn-Bottle Batch";
            var flav = "“Natural predator of a corrupt regime\"";
            var desc = "Inflict 1-3 Fire to all enemy positions at the beginning of combat.\nThis item is destroyed upon activation and gives you 5 more fire bombs.";

            var item = ItemBuilder.NewItem<PerformEffectWearable>("BurnBottleBatch_TW")
                .SetBasicInformation(name, flav, desc, "BurnBottleBatch")
                .SetStartsLocked(true)
                .AddItemTypes(ItemType.Fabric)
                .SetPrice(7)
                .AddToShop();

            item.usesTheOnUnlockText = false;

            var extraLoot = CreateScriptable<ExtraLootOptionsEffect>();
            extraLoot._itemName = "VyacheslavsLastSip_SW";
            extraLoot._changeOption = false;

            item.triggerOn = TriggerCalls.OnFirstTurnStart;
            item.effects =
            [
                Effects.Effect(null, CreateScriptable<ExtraVariableForNextEffect>(), 1),
                Effects.Effect(Targets.OpponentSide, CreateScriptable<ApplyRandomFireBetweenPreviousAndEntryEffect>(), 1),

                Effects.Effect(null, extraLoot),
                Effects.Effect(null, extraLoot),
                Effects.Effect(null, extraLoot),
                Effects.Effect(null, extraLoot),
                Effects.Effect(null, extraLoot),
            ];
            item._immediateEffect = false;
            item.doesItemPopUp = true;
            item.getsConsumedOnUse = true;
        }
    }
}
