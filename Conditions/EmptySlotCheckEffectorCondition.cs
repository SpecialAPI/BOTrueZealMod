using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Conditions
{
    public class EmptySlotCheckEffectorCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            var stats = CombatManager.Instance._stats;
            var slots = stats.combatSlots;
            var chSlots = slots.CharacterSlots;

            foreach(var slot in chSlots)
            {
                if (slot != null && !slot.HasUnit)
                    return true;
            }

            return false;
        }
    }
}
