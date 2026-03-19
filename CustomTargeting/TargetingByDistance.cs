using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.CustomTargeting
{
    public class TargetingByDistance : Targetting_ByUnit_Side
    {
        public bool getFurthest;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            var unitSlots = slots.GetAllUnitTargetSlotsAsList((isCasterCharacter && getAllies) || (!isCasterCharacter && !getAllies), getAllUnitSlots, ignoreCastSlot ? casterSlotID : (-1));
            var selfSlots = slots.GetAllSelfSlots(casterSlotID, isCasterCharacter);
            var leftmostSelfSlot = 5;
            var rightmostSelfSlot = -1;

            foreach (var s in selfSlots)
            {
                if (leftmostSelfSlot > s.SlotID)
                    leftmostSelfSlot = s.SlotID;

                if (rightmostSelfSlot < s.SlotID)
                    rightmostSelfSlot = s.SlotID;
            }

            selfSlots.Clear();
            var maxDist = -1;

            foreach (var s in unitSlots)
            {
                if (!s.HasUnit)
                    continue;

                var leftmostTargetSlot = s.SlotID;
                var rightmostTargetSlot = s.SlotID;

                if (!getAllUnitSlots)
                {
                    leftmostTargetSlot = s.Unit.SlotID;
                    rightmostTargetSlot = s.Unit.SlotID + s.Unit.Size - 1;
                }

                var dist = Mathf.Min(Mathf.Abs(leftmostTargetSlot - rightmostSelfSlot), Mathf.Abs(rightmostTargetSlot - leftmostSelfSlot));
                if (maxDist < 0 || dist == maxDist)
                {
                    maxDist = dist;
                    selfSlots.Add(s);
                }
                else if (getFurthest && dist > maxDist)
                {
                    selfSlots.Clear();
                    maxDist = dist;
                    selfSlots.Add(s);
                }
                else if (!getFurthest && dist < maxDist)
                {
                    selfSlots.Clear();
                    maxDist = dist;
                    selfSlots.Add(s);
                }
            }

            return [..selfSlots];
        }
    }
}
