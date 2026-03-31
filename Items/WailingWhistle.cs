using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Items
{
    public static class WailingWhistle
    {
        public static void Init()
        {
            var name = "Wailing Whistle";
            var flav = "\"Come and See\"";
            var desc = "This party member loses all passives but is inflicted with 1 Scar per turn.";

            var item = ItemBuilder.NewItem<PerformEffectWearable>("WailingWhistle_SW")
                .SetBasicInformation(name, flav, desc, "WailingWhistle")
                .SetPrice(4)
                .SetStartsLocked(true)
                .AddToShop();

            item.usesTheOnUnlockText = false;

            item.triggerOn = TriggerCallsE.OnTurnStart_Early;
            item.effects =
            [
                Effects.Effect(Targets.Self, CreateScriptable<RemoveAllPassivesEffect>(x =>
                {
                    x.disconnect = true;
                    x.removeFromExtras = true;
                }), 0),
                Effects.Effect(Targets.Self, CreateScriptable<ApplyScarsEffect>(), 1)
            ];
            item._immediateEffect = true;
            item.doesItemPopUp = true;
            item.conditions = [];
            item.getsConsumedOnUse = false;
        }
    }
}