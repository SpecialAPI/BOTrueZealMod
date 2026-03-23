using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Items
{
    public static class SacredShrub
    {
        public static void Init()
        {
            var name = "Sacred Shrub";
            var flav = "\"Alleged distant cousin of the Tree of Knowledge.\"";
            var desc = "Apply Divine Protection to all party members and enemies, exacerbating enemy weakness and bolstering party strengths.";

            var item = ItemBuilder.NewItem<PerformEffectWearable>("SacredShrub_TW")
                .SetBasicInformation(name, flav, desc, "SacredShrub")
                .SetPrice(0)
                .SetStartsLocked(true)
                .AddItemTypes(ItemType.Face, ItemType.Magic)
                .AddToTreasure();

            item.usesTheOnUnlockText = true;

            item.triggerOn = TriggerCalls.OnFirstTurnStart;
            item.effects =
            [
                Effects.Effect(Targets.AllOpponents, CreateScriptable<SacredShrubEffect>(x =>
                {
                    x.strongestToWeakest = false;
                    x.increment = 1;
                }), 0),
                Effects.Effect(Targets.AllAllies, CreateScriptable<SacredShrubEffect>(x =>
                {
                    x.strongestToWeakest = true;
                    x.increment = 1;
                }), 0),
            ];
        }
    }
}
