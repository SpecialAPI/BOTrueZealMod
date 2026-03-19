using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.CustomEffects
{
    public class IncreaseOnlyStatusEffectsEffect : EffectSO
    {
        public bool positive;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            if (entryVariable <= 0)
                return false;

            for (var i = 0; i < targets.Length; i++)
            {
                if (!targets[i].HasUnit)
                    continue;

                exitAmount += targets[i].Unit.IncreaseStatusEffects(entryVariable, positive);
            }

            return exitAmount > 0;
        }
    }
}
