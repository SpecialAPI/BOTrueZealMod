using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.CustomEffects
{
    public class RelapseEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            var healAmt = 0;
            var dmgAmt = 0;

            foreach(var t in targets)
            {
                if(!t.HasUnit)
                    continue;

                var u = t.Unit;

                if (u.CurrentHealth > caster.CurrentHealth)
                    dmgAmt += u.Damage(caster.WillApplyDamage(entryVariable, u), caster, DeathType.Basic, t.SlotID - u.SlotID, true, true, false, DamageType.None).damageAmount;

                else if (u.CurrentHealth < caster.CurrentHealth)
                    healAmt += u.Heal(entryVariable, HealType.Heal, true);
            }

            if (dmgAmt > 0)
                caster.DidApplyDamage(dmgAmt);

            exitAmount = healAmt + dmgAmt;
            return exitAmount > 0;
        }
    }
}
