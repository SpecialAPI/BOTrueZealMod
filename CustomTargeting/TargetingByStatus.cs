using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.CustomTargeting
{
    public class TargetingByStatus : Targetting_ByUnit_Side
    {
        public bool getStatus = true;
        public bool specificStatusOnly;
        public List<StatusEffectType> specificStatus = [];

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            var unitSlots = slots.GetAllUnitTargetSlotsAsList((isCasterCharacter && getAllies) || (!isCasterCharacter && !getAllies), getAllUnitSlots, ignoreCastSlot ? casterSlotID : (-1));
            for (var i = unitSlots.Count - 1; i >= 0; i--)
            {
                var target = unitSlots[i];
                if (!target.HasUnit)
                {
                    unitSlots.RemoveAt(i);
                    continue;
                }

                var hasStatus = false;
                if (getStatus)
                    hasStatus = DoStatusCheck(target);

                if (!hasStatus)
                    unitSlots.RemoveAt(i);
            }

            return [..unitSlots];
        }

        private bool DoStatusCheck(TargetSlotInfo target)
        {
            if (specificStatusOnly)
            {
                foreach (var item in specificStatus)
                {
                    if (target.Unit.ContainsStatusEffect(item))
                    {
                        return true;
                    }
                }
                return false;
            }

            return target.Unit is IStatusEffector effector && effector.StatusEffects.Count > 0;
        }
    }
}
