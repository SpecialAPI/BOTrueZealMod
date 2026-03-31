using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.CustomEffects
{
    public class RemoveAllPassivesEffect : EffectSO
    {
        public bool disconnect = true;
        public bool removeFromExtras = true;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach(var t in targets)
            {
                if (!t.HasUnit)
                    continue;

                t.Unit.RemoveAllPassives(disconnect, removeFromExtras);
                exitAmount++;
            }

            return exitAmount > 0;
        }
    }
}
