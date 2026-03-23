using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Items
{
    public static class RoyalPine
    {
        public static void Init()
        {
            var name = "Royal Pine";
            var flav = "\"Bake em’ like a puppy in a hot car!\"";
            var desc = "Enemies are now also affected by overflow damage.";

            var item = ItemBuilder.NewItem<PerformEffectOnAttachWearable>("RoyalPine_TW")
                .SetBasicInformation(name, flav, desc, "RoyalPine")
                .SetPrice(0)
                .SetStartsLocked(true)
                .AddToTreasure();

            item.usesTheOnUnlockText = false;

            item.triggerOn = TriggerCalls.OnCombatStart;
            item.attachEffects = [Effects.Effect(null, CreateScriptable<AddOrRemoveEnemyOverflowSourceEffect>(x => x.add = true))];
            item.dettachEffects = [Effects.Effect(null, CreateScriptable<AddOrRemoveEnemyOverflowSourceEffect>(x => x.add = false))];
            item.doesItemPopUp = false;
            item.doesTriggerAttachedActionOnInitialization = true;
            item.getsConsumedOnUse = false;
        }
    }
}
