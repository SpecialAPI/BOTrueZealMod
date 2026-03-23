using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Items
{
    public class SacredShrubEffect : EffectSO
    {
        public int increment;
        public bool strongestToWeakest;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            if(!stats.statusEffectDataBase.TryGetValue(StatusEffectType.DivineProtection, out var divineProtInfo))
                return false;

            var validTargets = new List<TargetSlotInfo>();
            foreach(var t in targets)
            {
                if(t != null && t.HasUnit && t.Unit.IsAlive)
                    validTargets.Add(t);
            }

            var orderedTargets = strongestToWeakest ?
                validTargets.OrderByDescending(x => x.Unit.CurrentHealth) :
                validTargets.OrderBy(x => x.Unit.CurrentHealth);

            var currDivineAmt = entryVariable;
            var currHealth = -1;

            foreach(var t in orderedTargets)
            {
                var u = t.Unit;

                if(currHealth == -1)
                    currHealth = u.CurrentHealth;
                else if (currHealth != u.CurrentHealth)
                {
                    currHealth = u.CurrentHealth;
                    currDivineAmt += increment;
                }

                if (currDivineAmt <= 0)
                    continue;

                var divineProt = new DivineProtection_StatusEffect(currDivineAmt);
                divineProt.SetEffectInformation(divineProtInfo);
                if (u.ApplyStatusEffect(divineProt, currDivineAmt))
                    exitAmount++;
            }

            return exitAmount > 0;
        }
    }
}
