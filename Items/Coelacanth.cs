using BOTrueZealMod.Conditions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Items
{
    public static class Coelacanth
    {
        public static void Init()
        {
            var name = "Coelacanth";
            var flav = "\"You Caught a... Coelacanth! 180cm\"";
            var desc = "On turn start, resurrect a random party member that has fallen in this combat encounter.\n60% chance to destroy this item upon activation.";

            var item = ItemBuilder.NewItem<PerformEffectWearable>("Coelacanth_ExtraW")
                .SetBasicInformation(name, flav, desc, "Coelacanth")
                .SetPrice(5)
                .SetStartsLocked(true)
                .AddToFishPool(5, 7);

            item.hasSpecialUnlock = true;
            item.unlockTextID = UILocID.ItemFishLocationLabel;
            item.usesTheOnUnlockText = false;

            item.triggerOn = TriggerCalls.OnTurnStart;
            item.effects = [Effects.Effect(Targets.AllySide, CreateScriptable<ResurrectInRandomSlotEffect>(), 1)];
            item._immediateEffect = false;
            item.doesItemPopUp = true;
            item.conditions = [CreateScriptable<EmptySlotCheckEffectorCondition>(), CreateScriptable<AvailableResurrectCharactersCheckEffectorCondition>()];
            item.getsConsumedOnUse = true;
            item.consumeConditions = [CreateScriptable<PercentageEffectorCondition>(x => x.triggerPercentage = 60)];
        }
    }
}
